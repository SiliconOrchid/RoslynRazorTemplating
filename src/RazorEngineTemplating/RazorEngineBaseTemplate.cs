using System.Text;
using System.Threading.Tasks;

namespace RazorEngineTemplating
{
    public abstract class RazorEngineBaseTemplate<TTemplateViewModel>
    {
        private readonly StringBuilder stringBuilder = new StringBuilder();

        public TTemplateViewModel Model;

        public void WriteLiteral(string literal)
        {
            stringBuilder.Append(literal);
        }

        public void Write(object obj)
        {
            stringBuilder.Append(obj.ToString());
        }


        public void BeginWriteAttribute(string name, string prefix, int prefixOffset, string suffix, int suffixOffset, int attributeValuesCount)
         {
            stringBuilder.Append($" {name}=");
        }

        public void WriteAttributeValue(string prefix, int prefixOffset, object value, int valueOffset, int valueLength, bool isLiteral)
        {
            stringBuilder.Append($"'{value}'");
        }

        public  void EndWriteAttribute()
        {
            
        }

        public string GetMarkup()
        {
            return stringBuilder.ToString();
        }

        public async virtual Task ExecuteAsync()
        {
            await Task.Yield();
        }
    }
}
