using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.media;
using umbraco.cms.businesslogic.web;
using umbraco.uicontrols;

using umbraco.presentation.masterpages;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.IO;
using System.Diagnostics;
using umbraco.cms.businesslogic.property;
using umbraco.cms.businesslogic.datatype;
using umbraco.BasePages;
using System.Configuration;

namespace uClamAV
{
 class EventHandlers : ApplicationBase
    {
        private Control control;
        private StringBuilder html_msg = new StringBuilder();

        /// <summary>umbraco event
        /// </summary>
              public EventHandlers()
                    {
                         umbraco.presentation.masterpages.umbracoPage.Load += new umbraco.presentation.masterpages.MasterPageLoadHandler(umbracoPage_Load);
                        if (is_active())
                        {
                            Media.AfterSave += new Media.SaveEventHandler(Media_BeforeSave);
                            Document.AfterSave += new Document.SaveEventHandler(Document_BeforeSave);
                        }

                    }

             // <summary>umbraco Media BeforeSav event: Media fiel prüfen
             /// </summary>
              void Media_BeforeSave(Media sender, umbraco.cms.businesslogic.SaveEventArgs e)
                    {
                        try
                        {
                            Config config = Config.GetConfig();
                            foreach (ConfigDatatype author in config.Datatypes)
                            {

                                foreach (Property pr in sender.GenericProperties)
                                {
                                    if (author.Guid == pr.PropertyType.DataTypeDefinition.DataType.Id)
                                    {

                                        Property fileprop = sender.getProperty(pr.PropertyType.Alias);
                                        e.Cancel = File_Scanner(fileprop);

                                    }

                                }
                            }

                        }
                        catch (Exception ex)
                        {

                        }

                        if (html_msg.Length > 0)
                        {
                            HtmlContent(html_msg);
                            html_msg.Clear();
                        }

                    }

              /// <summary> Document_BeforeSave(Document sender, umbraco.cms.businesslogic.SaveEventArgs e) umbraco  Document BeforeSave event :Document prüfen
              /// </summary>
              void Document_BeforeSave(Document sender, umbraco.cms.businesslogic.SaveEventArgs e)
                    {
                        try
                        {
                            Config config = Config.GetConfig();

                            foreach (ConfigDatatype author in config.Datatypes)
                            {

                                foreach (Property pr in sender.GenericProperties)
                                {
                                    if (author.Guid == pr.PropertyType.DataTypeDefinition.DataType.Id)
                                    {

                                        Property fileprop = sender.getProperty("" + pr.PropertyType.Alias + "");

                                        e.Cancel = File_Scanner(fileprop);

                                    }
                                }
                            }


                        }
                        catch (Exception ex)
                        {

                        }
                        if (html_msg.Length > 0)
                        {
                            HtmlContent(html_msg);
                            html_msg.Clear();
                        }

                    }

              /// <summary>File_Scanner(Property fileprop) prüft  fileprop
              /// <para> umraco fiel (Property fileprop)</para>
              /// </summary>
              bool File_Scanner(Property fileprop)
                    {

                        bool re_bool = false;
                        StringBuilder strFileInfo = new StringBuilder();
                        HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                        Config config = Config.GetConfig();
                        foreach (string hfile in files)
                        {
                            HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[hfile] as HttpPostedFile;
                            if (file != null)
                            {

                                string file_path = System.Web.HttpContext.Current.Server.MapPath(fileprop.Value.ToString());
                                FileInfo fileinfo = new FileInfo(fileprop.Value.ToString());
                                string file_thumb_path = Path.GetDirectoryName(file_path) + "\\" + Path.GetFileNameWithoutExtension(fileinfo.Name) + "_thumb" + fileinfo.Extension;

                                if (File.Exists(file_path))
                                {
                                    int clam_exitCode = RunClamAV(file_path);
                                    //switch für 1,2,3 ...
                                    switch (clam_exitCode)
                                    {
                                        case 0:
                                            re_bool = false;
                                            break;
                                        case 1:
                                            this.html_msg.Append(fileprop.Value.ToString() + " "+  get_virusfound_message() +"<br/>");

                                            try
                                            {
                                                File.Delete(file_path);
                                                if (File.Exists(file_thumb_path))
                                                {
                                                    File.Delete(file_thumb_path);
                                                }

                                            }
                                            catch (IOException)
                                            {
                                                umbraco.BasePages.BasePage.Current.ClientTools.ShowSpeechBubble(umbraco.BasePages.BasePage.speechBubbleIcon.info, System.Web.HttpContext.Current.Request.Files.Count.ToString() + " Files uploaded", "ERORR");
                                            }

                                            send_mail(file_path);
                                            re_bool = true;
                                            break;

                                        default:
                                            foreach (ConfigErrorcode er_code in config.Errorcode)
                                            {
                                                if (er_code.Key == clam_exitCode.ToString())
                                                {
                                                    umbraco.BasePages.BasePage.Current.ClientTools.ShowSpeechBubble(umbraco.BasePages.BasePage.speechBubbleIcon.error, "uClamAV", er_code.Text);
                                                }
                                            }

                                            break;
                                    }


                                }
                            }

                        }

                        return re_bool;
                    }

              /// <summary>umbracoPage_Load(object sender, EventArgs e) umbracoPage_Load event  sucht nach Controle und prüft ob uclamAv active ist
             /// </summary>
              void umbracoPage_Load(object sender, EventArgs e)
                    {
                        html_msg.Clear();
                        Config config = new Config(); run_freshclam();
                        umbracoPage up = (umbracoPage)sender;
                        StringBuilder msg = new StringBuilder();
                        ContentPlaceHolder cph = (ContentPlaceHolder)up.FindControl("body");
                        Control c2 = cph.FindControl("TabView1");
                        this.control = c2;


                        if (c2 != null && !is_active())
                        {
                            msg.Append(get_notactive_message());
                            HtmlContent(msg);

                        }


                    }

              /// <summary>HtmlContent(StringBuilder html)
              /// <para> StringBuilde :Error message </para>
              /// </summary>
              void HtmlContent(StringBuilder html)
                    {
                        if(html.ToString()!=""){
                        StringBuilder csHtml = new StringBuilder();
                        csHtml.Append("<div class=\"propertypane\" style=\"border:1px solid red;color:red;margin:10px;\"><div><div  class=\"propertyItem\"></div>" + html.ToString() + "</div></div>");
                        this.control.Controls.Add(new System.Web.UI.LiteralControl("<script>$('" + csHtml.ToString() + "').insertBefore('.tabpageContent')</script>"));
                        }
                    }


              /// <summary>RunClamAV(String fullpath)start  clamscan.exe
              /// <para>Media path</para>
              /// <seealso cref="TestClass.Main"/>
              /// </summary>
              int RunClamAV(String fullpath)
                    {

                        try
                        {
                            Process myProcess = new Process();
                            Config config = Config.GetConfig();
                            if (File.Exists(System.Web.HttpContext.Current.Server.MapPath(umbraco.GlobalSettings.Path + "/.." +config.Clam_path)))
                            {
                                myProcess.StartInfo.FileName = myProcess.StartInfo.FileName = System.Web.HttpContext.Current.Server.MapPath(umbraco.GlobalSettings.Path + "/.." + config.Clam_path); ;
                                string myprocarg = "\"" + @fullpath + "\"";
                                myProcess.StartInfo.Arguments = myprocarg;
                                myProcess.StartInfo.UseShellExecute = false;
                                myProcess.StartInfo.CreateNoWindow = true;
                                myProcess.StartInfo.RedirectStandardOutput = true;
                                myProcess.Start();
                                myProcess.WaitForExit();
                                return myProcess.ExitCode;
                            }
                            else
                            {
                                return -1;
                            }


                        }
                        catch (Exception ex)
                        {
                            umbraco.BasePages.BasePage.Current.ClientTools.ShowSpeechBubble(umbraco.BasePages.BasePage.speechBubbleIcon.error, " ERORR", ex.Message);
                            return 3;
                        }
                    }

              /// <summary>is_active prüft ob uclamAv active ist
              /// </summary>
              bool is_active()
                    {
                        Config config = Config.GetConfig();
                        if (config.Active == "true")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

              /// <summary>get_notactive_message.liest noactive message von uClamAv.config
              /// </summary>
            string get_notactive_message()
                    {
                        string notactive = "";
                        Config config = Config.GetConfig();
                        HttpResponse response = System.Web.HttpContext.Current.Response;
                        response.Clear();

                        foreach (ConfigMessage s in config.Message)
                        {
                            if (s.Key == "notactive")
                            {
                                return s.Text;
                            }
                        }
                        return notactive;

                    }

            /// <summary>get_virusfound_message liest email text von UclamAv.config
            /// </summary>
            string get_virusfound_message()
                    {
                        string virusfound = "";
                        Config config = Config.GetConfig();
                        HttpResponse response = System.Web.HttpContext.Current.Response;
                        response.Clear();

                        foreach (ConfigMessage s in config.Message)
                        {
                            if (s.Key == "virusfound")
                            {
                                return s.Text;
                            }
                        }
                        return virusfound;

                    }
            /// <summary>send_mail  sendet ein email .
            /// <para>media path</para>
            /// <seealso cref="TestClass.Main"/>
            /// </summary>

        void send_mail(string file_path)
                    {
                        Config config = Config.GetConfig();
                        StringBuilder msg_mail = new StringBuilder();
                        msg_mail.AppendLine(get_virusfound_message());
                        msg_mail.AppendLine(file_path);
                        try
                        {
                            umbraco.library.SendMail(config.From_mail, config.To_mail, "uClamAV ", msg_mail.ToString(), false);
                        }
                        catch (Exception ex)
                        {
                            this.html_msg.Append("Send mail error : " + ex.ToString() + "<br/>");
                        }
                    }


        /// <summary>run_freshclam
        /// prüft ob db aktualisiert
        /// </summary>
        void run_freshclam()
                    {
          
                        var config = Config.GetConfig();
                        //System.Web.HttpContext.Current.Response.Write(config.Freshclam_path);
                        //System.Web.HttpContext.Current.Response.End();
                        if (File.Exists(System.Web.HttpContext.Current.Server.MapPath(config.Freshclam_path)))
                        {
                            try
                            {

                                long ticksvalue = 0;
                                if (!Directory.Exists(Directory.GetParent(System.Web.HttpContext.Current.Server.MapPath(umbraco.GlobalSettings.Path + "/.." + config.Freshclam_path)).FullName + "/db"))
                                {
                                    Directory.CreateDirectory(Directory.GetParent(System.Web.HttpContext.Current.Server.MapPath(umbraco.GlobalSettings.Path + "/.." + config.Freshclam_path)).FullName + "/db");
                                }

                                string dirName = Directory.GetParent(System.Web.HttpContext.Current.Server.MapPath(umbraco.GlobalSettings.Path + "/.." + config.Freshclam_path)).FullName + "/uClamAVfresh.data";
                                if (File.Exists(dirName))
                                {
                                    FileInfo fileInfo = new FileInfo(dirName);
                                    ticksvalue = fileInfo.LastWriteTime.Ticks;
                                    int time_intervall = 6;
                                    if (!Int32.TryParse(config.Freshclamintervall, out time_intervall))
                                    {
                                        time_intervall = 0;
                                    }
                                    if ((new DateTime(ticksvalue).AddHours(time_intervall).Ticks) < DateTime.Now.Ticks)
                                    {


                                        File.Delete(dirName);
                                        System.IO.FileStream f = System.IO.File.Create(dirName);
                                        f.Close();

                                        run_Freshclam_pross();
                                    }

                                }
                                else
                                {
                                    System.IO.FileStream f = System.IO.File.Create(dirName);
                                    f.Close();
                                    run_Freshclam_pross();
                                }
                            }
                            catch (Exception ex)
                            {
                                umbraco.BasePages.BasePage.Current.ClientTools.ShowSpeechBubble(umbraco.BasePages.BasePage.speechBubbleIcon.error, " ERORR", ex.Message);

                            }
                        }
                    }

        /// <summary>run Freshclam.exe
        /// </summary>
        void  run_Freshclam_pross()
                    {
           
                        try
                        {
                            Config config = Config.GetConfig();
              
                
                            Process myProcess = new Process();
                            if (File.Exists(System.Web.HttpContext.Current.Server.MapPath(umbraco.GlobalSettings.Path + "/.." + config.Freshclam_path)))
                            {
                                myProcess.StartInfo.FileName = myProcess.StartInfo.FileName = System.Web.HttpContext.Current.Server.MapPath(umbraco.GlobalSettings.Path + "/.." + config.Freshclam_path);
                                myProcess.Start();
                                myProcess.StartInfo.UseShellExecute = false;
                                myProcess.StartInfo.CreateNoWindow = true;
                                myProcess.StartInfo.RedirectStandardOutput = true;

                            }


                        }
                        catch (Exception ex)
                        {
                            umbraco.BasePages.BasePage.Current.ClientTools.ShowSpeechBubble(umbraco.BasePages.BasePage.speechBubbleIcon.error, " ERORR", ex.Message);

                        }

                    }
    }
}
