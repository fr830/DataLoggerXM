using DataLogger.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataLogger.Utils
{

    public enum ELanguage
    {
        English,
        Vietnamese
    }

    public enum EAlign
    {
        None,
        Left,
        Right,
        Center
    }

    public class LanguageInfo
    {
        public ELanguage Language { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Bitmap Icon { get; set; }
    }

    public class LanguageService
    {

        public ResourceManager resourceManager { get; set; }    // declare Resource manager to access to specific cultureinfo

        public CultureInfo cultureInfo { get; set; }           //declare culture info

        public LanguageInfo CurrentLanguage { get; set; }

        private List<LanguageInfo> languageList = new List<LanguageInfo>
        {
            //new LanguageInfo{ Language = ELanguage.English, Code = "en", Name = "English", Icon = Resources.en },
            //new LanguageInfo{ Language = ELanguage.Vietnamese, Code = "vi", Name = "Tiếng Việt", Icon = Resources.Flag_of_Vietnam_43x32 }

            new LanguageInfo{ Language = ELanguage.English, Code = "en", Name = "France", Icon = DataLogger.Properties.Resources.Flag_of_Vietnam_43x32},
            new LanguageInfo{ Language = ELanguage.Vietnamese, Code = "fa", Name = "English", Icon = DataLogger.Properties.Resources.en  }
        };

        public LanguageService()
        {
        }

        public LanguageService(Assembly assembly)
        {
            CurrentLanguage = languageList[0];
            resourceManager = new ResourceManager("DataLogger.Resources.Res", assembly);
            cultureInfo = CultureInfo.CreateSpecificCulture(CurrentLanguage.Code);
        }

        public void switchLanguageTo(ELanguage lang)
        {
            CurrentLanguage = languageList.FirstOrDefault(l => l.Language == lang);
            cultureInfo = CultureInfo.CreateSpecificCulture(CurrentLanguage.Code);
        }

        public void nextLanguage()
        {
            switch (CurrentLanguage.Language)
            {
                case ELanguage.English:
                    this.switchLanguageTo(ELanguage.Vietnamese);
                    break;
                case ELanguage.Vietnamese:
                    this.switchLanguageTo(ELanguage.English);
                    break;
                default:
                    break;
            }
        }

        public string getText(string langKey)
        {
            return resourceManager.GetString(langKey, cultureInfo);
        }

        public void setText(Control control, string langKey)
        {
            if (control is Label)
            {
                ((Label)control).Text = this.getText(langKey);
            }else            if (control is Button)
            {
                ((Button)control).Text = this.getText(langKey);
            }
            else if (control is Form)
            {
                ((Form)control).Text = this.getText(langKey);
            }
            else if (control is GroupBox)
            {
                ((GroupBox)control).Text = this.getText(langKey);
            }
        }

        public void setText(Control control, string langKey, EAlign align)
        {
            this.setText(control, langKey);

            switch (align)
            {
                case EAlign.None:
                    break;
                case EAlign.Left:
                    control.Left = 0;
                    break;
                case EAlign.Right:
                    control.Left = control.Parent.Width - control.Width;
                    break;
                case EAlign.Center:
                    control.Left = (control.Parent.Width - control.Width) / 2;
                    break;
                default:
                    break;
            }
        }

        public void setText(Control control, string langKey, Control refControl, EAlign align)
        {
            this.setText(control, langKey);

            switch (align)
            {
                case EAlign.None:
                    break;
                case EAlign.Left:
                    control.Left = refControl.Left;
                    break;
                case EAlign.Right:
                    control.Left = refControl.Left + refControl.Width - control.Width;
                    break;
                case EAlign.Center:
                    control.Left = refControl.Left + (refControl.Width - control.Width) / 2;
                    break;
                default:
                    break;
            }
        }
    }


}
