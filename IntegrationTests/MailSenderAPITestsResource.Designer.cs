﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IntegrationTests {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class MailSenderAPITestsResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal MailSenderAPITestsResource() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("IntegrationTests.MailSenderAPITestsResource", typeof(MailSenderAPITestsResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {
        ///   &quot;subject&quot;:&quot;Hello&quot;,
        ///   &quot;body&quot;:&quot;Body here&quot;,
        ///   &quot;recipients&quot;:[
        ///      &quot;ta.nya.smith1712@gmail.com&quot;,
        ///      &quot;tanya.smith1712@gmail.com&quot;
        ///   ]
        ///}.
        /// </summary>
        public static string CorrectMessage {
            get {
                return ResourceManager.GetString("CorrectMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на [{&quot;Subject&quot;:&quot;test_subject&quot;,&quot;Body&quot;:&quot;test_body&quot;,&quot;Recipients&quot;:[&quot;first@gmail.com&quot;,&quot;second@gmail.com&quot;],&quot;date&quot;:&quot;01.01.0001 3:00&quot;,&quot;Result&quot;:&quot;test_result&quot;,&quot;FailedMessage&quot;:&quot;test_message&quot;}].
        /// </summary>
        public static string CorrectMessages {
            get {
                return ResourceManager.GetString("CorrectMessages", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {  
        ///&quot;subject&quot;:&quot;Subject here&quot;,
        ///   &quot;body&quot;:&quot;Body here&quot;,
        ///&quot;recipients&quot;: []
        ///}.
        /// </summary>
        public static string IncorrectMessage_EmptyRecipients {
            get {
                return ResourceManager.GetString("IncorrectMessage.EmptyRecipients", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {  
        ///   &quot;subject&quot;:&quot;Subject here&quot;,
        ///   &quot;recipients&quot;:[
        ///      &quot;ta.nya.smith1712@gmail.com&quot;,
        ///      &quot;tanya.smith1712@gmail.com&quot;
        ///   ]
        ///}.
        /// </summary>
        public static string IncorrectMessage_NullBody {
            get {
                return ResourceManager.GetString("IncorrectMessage.NullBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {  
        ///&quot;subject&quot;:&quot;Subject here&quot;,
        ///   &quot;body&quot;:&quot;Body here&quot;
        ///}.
        /// </summary>
        public static string IncorrectMessage_NullRecipients {
            get {
                return ResourceManager.GetString("IncorrectMessage.NullRecipients", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {  
        ///   &quot;body&quot;:&quot;Body here&quot;,
        ///   &quot;recipients&quot;:[
        ///      &quot;ta.nya.smith1712@gmail.com&quot;,
        ///      &quot;tanya.smith1712@gmail.com&quot;
        ///   ]
        ///}.
        /// </summary>
        public static string IncorrectMessage_NullSubject {
            get {
                return ResourceManager.GetString("IncorrectMessage.NullSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {  
        ///&quot;subject&quot;:&quot;Subject here&quot;,
        ///   &quot;body&quot;:&quot;Body here&quot;,
        ///&quot;recipients&quot;:[
        ///      &quot;ta.nya.smith1712gmail.com&quot;,
        ///      &quot;tanya.smith1712@gmail.com&quot;
        ///   ]
        ///}.
        /// </summary>
        public static string MessageWithIncorrectEmailAddress {
            get {
                return ResourceManager.GetString("MessageWithIncorrectEmailAddress", resourceCulture);
            }
        }
    }
}
