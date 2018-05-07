using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TESTUpload.ServiceReference1;

namespace TESTUpload
{
    public partial class UploadTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

		
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            IJARAHSADADUPLOADClient objClient = new IJARAHSADADUPLOADClient();

            objClient.GetAndUploadAccountReceivables();
           
        }
    }
}