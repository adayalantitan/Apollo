using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Text;
using Titan.Email;

namespace Apollo
{
    public partial class emailTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void sendEmail_Click(object sender, EventArgs e)
        {
            try
            {
                Message message = new Message("172.18.0.121");
				/*
                message.message.From = new MailAddress("Nicks.NerfBall@titan360.com", "The Kidnapper");
				message.message.To.Add(new MailAddress("nick.akturk@titan360.com"));                
				message.message.Bcc.Add(new MailAddress("stephen.salamida@titan360.com"));
                message.message.Bcc.Add(new MailAddress("john.klett@titan360.com"));
                message.message.Bcc.Add(new MailAddress("dean.nguyen@titan360.com"));
                message.message.Bcc.Add(new MailAddress("marisol.monsanto@titan360.com"));
                message.message.Bcc.Add(new MailAddress("woody.nitibhon@titan360.com"));
                message.message.Bcc.Add(new MailAddress("bianka.faustin@titan360.com"));
                message.message.Bcc.Add(new MailAddress("carlos.vasquez@titan360.com"));
                message.message.Bcc.Add(new MailAddress("sdsalamida@gmail.com"));                              
                message.AddAttachmentFromFile(Server.MapPath(@"~/mktbbpic/aliens.jpg"), "image/jpeg");
                message.AddAttachmentFromFile(Server.MapPath(@"~/mktbbpic/atlas.jpg"), "image/jpeg");
                message.AddAttachmentFromFile(Server.MapPath(@"~/mktbbpic/basic.jpg"), "image/jpeg");
                message.AddAttachmentFromFile(Server.MapPath(@"~/mktbbpic/knoll.jpg"), "image/jpeg");
                message.AddAttachmentFromFile(Server.MapPath(@"~/mktbbpic/linsanity.jpg"), "image/jpeg");
                message.AddAttachmentFromFile(Server.MapPath(@"~/mktbbpic/marilyn.jpg"), "image/jpeg");
                message.AddAttachmentFromFile(Server.MapPath(@"~/mktbbpic/minnesotafats.jpg"), "image/jpeg");
                message.AddAttachmentFromFile(Server.MapPath(@"~/mktbbpic/monk.jpg"), "image/jpeg");
                message.AddAttachmentFromFile(Server.MapPath(@"~/mktbbpic/nessy.jpg"), "image/jpeg");
                message.AddAttachmentFromFile(Server.MapPath(@"~/mktbbpic/outbreak.jpg"), "image/jpeg");
                message.AddAttachmentFromFile(Server.MapPath(@"~/mktbbpic/pokemon.jpg"), "image/jpeg");
                message.AddAttachmentFromFile(Server.MapPath(@"~/mktbbpic/shot.jpg"), "image/jpeg");
                message.AddAttachmentFromFile(Server.MapPath(@"~/mktbbpic/spoon.jpg"), "image/jpeg");
                message.AddAttachmentFromFile(Server.MapPath(@"~/mktbbpic/super.jpg"), "image/jpeg");
                message.AddAttachmentFromFile(Server.MapPath(@"~/mktbbpic/thinker.jpg"), "image/jpeg");
                message.AddAttachmentFromFile(Server.MapPath(@"~/mktbbpic/winteriscoming.jpg"), "image/jpeg");
                message.Subject = "It's almost punch time...";
				*/
				message.message.From = new MailAddress("king.hippo@titan360.com", "Titan Orbitting Satellite Kupernicus 9-E Space-Weather Satellite...in Space");
				message.message.To.Add(new MailAddress("john.klett@titan360.com", "John 'Old Man' Klett"));
				message.message.Bcc.Add(new MailAddress("dean.nguyen@titan360.com"));
				message.message.Bcc.Add(new MailAddress("stephen.salamida@titan360.com"));
				message.message.Bcc.Add(new MailAddress("jonathan.vargas@titan360.com"));
                message.message.IsBodyHtml = true;
                StringBuilder messageBody = new StringBuilder();
                messageBody.AppendLine(@"<p align=""center""><b><font size=""+2"">Geomagnetic Sudden Impulse and Supernova Warning!!!!</font></b></p><p align=""left""><b><font size=""+1"">WARNING: Geomagnetic Sudden Impulse and/or Supernova expected</font></b></p><blockquote><p><b>Space-Weather Satellite Message Code: JUSTINBIEBER<br>		Serial Number: 001-0945712-b-hippo<br>		Issue Time: 2012 Apr 19 1930 UTC</b> </p>		<p><b>WARNING: Geomagnetic Sudden Impulse and/or Supernova expected<br>		Valid From: 2012 Apr 19 1650 UTC<br>		Valid To: 2012 Apr 20 1730 UTC<br>		</p>	</blockquote>	<p align=""left""><font size=""+1""><b>SUMMARY: Geomagnetic Sudden Impulse and/or Supernova</b></font></p>	<blockquote> 		<p><b>Space-Weather Satellite Message Code: JUSTINBIEBERSUM<br>		Serial Number: 001-0945712-b-hippo<br>		Issue Time: 2012 Apr 19 1930 UTC</b> </p>		<p><b>SUMMARY: Geomagnetic Sudden Impulse and/or Supernova expected<br>		Event to be localized to the Wilmington, DE area. Please update all digital screens immediately to relay information to the passenger(s).<br>Small children, cats, dogs, elderly people, and pregnant women should be abandoned at once. Head indoors imediately.</p></blockquote>");
                message.Body = messageBody.ToString();
                message.SendEmail();
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                Response.Write("Error");
            }
        }
    }
}