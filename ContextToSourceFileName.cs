using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace INT.BizTalk.PipelineComponents
{
    public partial class ContextToSourceFileName : IComponent
    {
        private const string BizTalkReceivedFileNameContext = "ReceivedFileName";
        private const string BizTalkFilePropertySchema = "http://schemas.microsoft.com/BizTalk/2003/file-properties";

        public IBaseMessage Execute(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            string receivedFileName = pInMsg.Context.Read(BizTalkReceivedFileNameContext, BizTalkFilePropertySchema) as string;
            string pattern = @"\[%([^%]+)%\]";
            var values = Regex.Matches(TargetSourceFileName, pattern);
            var propertySchemas = ContextPropertySchemas.Split('|');
            StringBuilder sourceFileNameBuilder = new StringBuilder(TargetSourceFileName);

            foreach (Match match in values)
            {
                string contextName = match.Groups[1].Value;
                string contextValue = "";

                foreach (string schema in propertySchemas)
                {
                    contextValue = pInMsg.Context.Read(contextName, schema) as string;

                    if (!string.IsNullOrWhiteSpace(contextValue))
                        break;
                }

                sourceFileNameBuilder.Replace(match.Value, contextValue);
            }

            if (!string.IsNullOrWhiteSpace(OverrideFileExtension))
                sourceFileNameBuilder.Append(OverrideFileExtension);
            else
                sourceFileNameBuilder.Append(Path.GetExtension(receivedFileName));

            pInMsg.Context.Write(BizTalkReceivedFileNameContext, BizTalkFilePropertySchema, sourceFileNameBuilder.ToString());
            return pInMsg;
        }
    }
}
