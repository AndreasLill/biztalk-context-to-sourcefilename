using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace INT.BizTalk.PipelineComponents
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [ComponentCategory(CategoryTypes.CATID_Any)]
    [System.Runtime.InteropServices.Guid("4338FA62-5D37-4F7C-B2FC-89E99CB5B543")]
    public class ContextToSourceFileName : IBaseComponent, IPersistPropertyBag, Microsoft.BizTalk.Component.Interop.IComponent
    {
        private const string BizTalkReceivedFileNameContext = "ReceivedFileName";
        private const string BizTalkFilePropertySchema = "http://schemas.microsoft.com/BizTalk/2003/file-properties";
		
		#region BaseComponent Configuration

        public string Name => "ContextToSourceFileName";

        public string Version => "1.0.0";

        public string Description => "Adds a context property to the SourceFileName macro.";

        [Description("The target SourceFileName using context macros [%ContextName%]. Example: [%ReceivePortName%]_[%CompanyId%]_othertext")]
        public string TargetSourceFileName { get; set; }

        [Description("Namespace of context property schemas to use. Can use multiple delimited by |")]
        public string ContextPropertySchemas { get; set; }

        [Description("Override and replace the current file extension. Leave blank to keep current extension.")]
        public string OverrideFileExtension { get; set; }

        public void GetClassID(out Guid classID)
        {
            classID = new Guid("4338FA62-5D37-4F7C-B2FC-89E99CB5B543");
        }

        public void InitNew()
        {
        }

        public void Load(IPropertyBag propertyBag, int errorLog)
        {
            object val = ReadPropertyBag(propertyBag, "TargetSourceFileName");
            if (val != null) TargetSourceFileName = val as string;

            val = ReadPropertyBag(propertyBag, "ContextPropertySchema");
            if (val != null) ContextPropertySchemas = val as string;

            val = ReadPropertyBag(propertyBag, "OverrideFileExtension");
            if (val != null) OverrideFileExtension = val as string;
        }

        public void Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties)
        {
            propertyBag.Write("TargetSourceFileName", TargetSourceFileName);
            propertyBag.Write("ContextPropertySchema", ContextPropertySchemas);
            propertyBag.Write("OverrideFileExtension", OverrideFileExtension);
        }

        private object ReadPropertyBag(IPropertyBag propertyBag, string propName)
        {
            object val = null;
            try
            {
                propertyBag.Read(propName, out val, 0);
            }
            catch (ArgumentException)
            {
                return val;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message, e);
            }
            return val;
        }
		
		#endregion

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
