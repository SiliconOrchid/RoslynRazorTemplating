
using System.Collections.Generic;

namespace EmailTemplateDemo.EmailTemplates
{
    public class SampleEmailTemplateModel
    {
        public string EmailTagline { get; set; }
        public List<SampleEmailTemplateModelCollectionItem> ListCollectionItems { get; set; }


        public SampleEmailTemplateModel()
        {
            ListCollectionItems = new List<SampleEmailTemplateModelCollectionItem>();
        }

    }



    public class SampleEmailTemplateModelCollectionItem
    {
        public string CollectionItemDescription { get; set; }
    }

}
