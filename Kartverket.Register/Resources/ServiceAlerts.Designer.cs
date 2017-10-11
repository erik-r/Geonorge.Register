﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ServiceAlerts {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ServiceAlerts() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Kartverket.Register.Resources.ServiceAlerts", typeof(ServiceAlerts).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
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
        ///   Looks up a localized string similar to Varslingsdato.
        /// </summary>
        public static string AlertDate {
            get {
                return ResourceManager.GetString("AlertDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Varslingsdato er påkrevd.
        /// </summary>
        public static string AlertDateErrorMessage {
            get {
                return ResourceManager.GetString("AlertDateErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Varslingsdato bør ikke settes bakover i tid.
        /// </summary>
        public static string AlertDateValidationMessage {
            get {
                return ResourceManager.GetString("AlertDateValidationMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Type varsel.
        /// </summary>
        public static string AlertType {
            get {
                return ResourceManager.GetString("AlertType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Valg av type varsel er påkrevd.
        /// </summary>
        public static string AlertTypeErrorMessage {
            get {
                return ResourceManager.GetString("AlertTypeErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ikrafttredelsesdato.
        /// </summary>
        public static string EffectiveDate {
            get {
                return ResourceManager.GetString("EffectiveDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ikrafttredelsesdato er påkrevd.
        /// </summary>
        public static string EffectiveDateErrorMessage {
            get {
                return ResourceManager.GetString("EffectiveDateErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Her velger du den datoen varselet er registrert i Geonorge. Systemet foreslår dagens dato. Når du trykker på «Publiser»-knappen blir varselet umiddelbart tilgjengelig for alle som følger tjenestevarsler, uavhengig av hvilken dato du har valgt i dette feltet..
        /// </summary>
        public static string HelpTextAlertdate {
            get {
                return ResourceManager.GetString("HelpTextAlertdate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Velg en type varsel fra nedtrekkslisten. Dersom du ikke finner en passende type varsel kan du sende en epost til &lt;a href=&quot;mailto:post@norgedigitalt.no&quot;&gt;post@norgedigitalt.no&lt;/a&gt; og fremme ditt ønske om nye typer varsler. Men husk at for brukerne som mottar varslene så må varseltype være så konkret og presis som mulig, samtidig som det kan være forvirrende med altfor mange ulike typer varsel. Ofte er det godt nok med en varseltype som dekker et litt større emne. Forsøk derfor å bruke de foreslåtte varseltype [rest of string was truncated]&quot;;.
        /// </summary>
        public static string HelpTextAlerttype {
            get {
                return ResourceManager.GetString("HelpTextAlerttype", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Her velger du den datoen hvor tjenesten vil bli endret. I Norge digitalt er det 3 måneders varslingstid for endring eller fjerning av tjenester. Systemet foreslår derfor en dato 3 måneder frem i tid for dette feltet..
        /// </summary>
        public static string HelpTextEffectivedate {
            get {
                return ResourceManager.GetString("HelpTextEffectivedate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Her skriver du inn hvilke konkrete endringer som vil bli gjort i tjenesten som dette varselet omfatter. Dersom varselet gjelder endring av en URL bør du oppgi hva den nye URLen vil bli.
        ///Dersom varselet gjelder endring i datakvalitet eller datastruktur, bør du opplyse hva disse endringene innebærer slik at brukere/utviklere kan oppdatere sin klienter i tråd med endringene.
        ///I alle tilfeller er det også lurt å oppgi årsaken til endringene, siden brukerne ofte blir mer forståelsesfulle for endringer dersom de [rest of string was truncated]&quot;;.
        /// </summary>
        public static string HelpTextNote {
            get {
                return ResourceManager.GetString("HelpTextNote", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Velg en tjeneste fra nedtrekkslisten. Du kan kun velge en tjeneste som allerede er registrert i Geonorge. Dersom tjenesten du skal legge inn nytt varsel for ikke finnes i nedtrekkslista, må du registrere tjenesten i Geonorge via metadataeditoren og din BAAT-bruker..
        /// </summary>
        public static string HelpTextServiceuuid {
            get {
                return ResourceManager.GetString("HelpTextServiceuuid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Siste varsel.
        /// </summary>
        public static string LastAlert {
            get {
                return ResourceManager.GetString("LastAlert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Varselet gjelder.
        /// </summary>
        public static string Note {
            get {
                return ResourceManager.GetString("Note", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Det er påkrevd å skrive hva varselet gjelder.
        /// </summary>
        public static string NoteErrorMessage {
            get {
                return ResourceManager.GetString("NoteErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Varselet gjelder på engelsk.
        /// </summary>
        public static string NoteTranslated {
            get {
                return ResourceManager.GetString("NoteTranslated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Velg tjeneste.
        /// </summary>
        public static string SelectService {
            get {
                return ResourceManager.GetString("SelectService", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Velg tjenestevarsel.
        /// </summary>
        public static string SelectServiceAlert {
            get {
                return ResourceManager.GetString("SelectServiceAlert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tjeneste.
        /// </summary>
        public static string Service {
            get {
                return ResourceManager.GetString("Service", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tjenestebeskrivelse.
        /// </summary>
        public static string ServiceDesription {
            get {
                return ResourceManager.GetString("ServiceDesription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tjenestenavn.
        /// </summary>
        public static string ServiceName {
            get {
                return ResourceManager.GetString("ServiceName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tjenestetype.
        /// </summary>
        public static string ServiceType {
            get {
                return ResourceManager.GetString("ServiceType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Valg av tjeneste er påkrevd.
        /// </summary>
        public static string ServiceTypeErrorMessage {
            get {
                return ResourceManager.GetString("ServiceTypeErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Abonnere på tjenestevarsler.
        /// </summary>
        public static string SubscribeServiceAlert {
            get {
                return ResourceManager.GetString("SubscribeServiceAlert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Foreslå nytt tjenestevarsel.
        /// </summary>
        public static string Title {
            get {
                return ResourceManager.GetString("Title", resourceCulture);
            }
        }
    }
}
