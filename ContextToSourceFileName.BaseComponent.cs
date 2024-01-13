using Microsoft.BizTalk.Component.Interop;
using System;
using System.ComponentModel;

namespace INT.BizTalk.PipelineComponents
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [ComponentCategory(CategoryTypes.CATID_Any)]
    [System.Runtime.InteropServices.Guid("4338FA62-5D37-4F7C-B2FC-89E99CB5B543")]
    public partial class ContextToSourceFileName : IBaseComponent, IPersistPropertyBag
    {
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
    }
}
