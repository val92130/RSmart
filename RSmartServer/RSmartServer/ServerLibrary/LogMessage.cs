using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Server.Lib
{
    public enum EMessageCategory
    {
        Default,
        Error,
        Information,
        Success
    }
    public class LogMessage
    {
        private string _text;
        private Color _color;
        private EMessageCategory _messageCategory;
        public LogMessage(String text)
        {
            _text = text;
            _color = Colors.White;
            _messageCategory = EMessageCategory.Default;
        }
        public LogMessage(String text, Color color)
        {
            _text = text;
            _color = color;
            _messageCategory = EMessageCategory.Default;
        }
        public LogMessage(String text, EMessageCategory category)
        {
            _text = text;
            _messageCategory = category;
            _color = GetColorCategory(category);
        }
        public LogMessage(String text, Color color, EMessageCategory category)
        {
            _text = text;
            _color = color;
            _messageCategory = category;
        }

        public Color Color
        {
            get
            {
                return _color;
            }
        }

        public String Text
        {
            get
            {
                return _text;
            }
        }

        public EMessageCategory Category
        {
            get { return _messageCategory; }
        }

        public override string ToString()
        {
            return _text;
        }

        private Color GetColorCategory(EMessageCategory category)
        {
            switch (category)
            {
                case EMessageCategory.Default:
                    return Colors.White;
                case EMessageCategory.Error:
                    return Colors.Red;
                case EMessageCategory.Information:
                    return Colors.Yellow;
                case EMessageCategory.Success:
                    return Colors.LawnGreen;
                default:
                    return Colors.White;
            }
        }
    }
}
