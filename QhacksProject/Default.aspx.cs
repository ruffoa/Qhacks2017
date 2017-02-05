using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using QhacksProject.classes;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;

namespace QhacksProject
{
    public partial class _Default : Page
    {
        public string question = "";
        string[] ans = new string[3];
        int ctr = 1;
        int hops = 0; int malt = 0; int yeast = 0;
        static string resultStr = "";

        public class StringTable
        {
            public string[] ColumnNames { get; set; }
            public string[,] Values { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {

                Session["ans0"] = "";
                Session["ans1"] = "";
                Session["ans2"] = "";
                Session["num"] = null;

            }
            // When the user clicks a button on the page (posts), use the
            // node in session instead
            else
            {
                if (Session["num"] != null)
                {
                    Int32.TryParse(Session["num"].ToString(), out ctr);
                }
            }
            giveInf(1);
            questionLabel.Text = question;
        }


        public string giveInf(int num)
        {
            question = "";
            if (num == 1)
            {
                question = "How much hops do you have? (1-10)";
            }
            if (num == 2)
            {
                question = "How much yeast do you have? (1-10)";

            }
            if (num == 3)
            {
                question = "How much malt do you want? (1-10)";

            }
            return question;
        }

        protected void goButton_Click(object sender, EventArgs e)
        {
            if (ctr < 3)
            {
                int res = 0;
                if (Int32.TryParse(usrInputField.Text, out res) && usrInputField.Text != "")
                {
                    if (Session["ans0"] != null)
                    {
                        ans[0] = Session["ans0"].ToString();
                    }
                    if (Session["ans1"] != null)
                    {
                        ans[1] = Session["ans1"].ToString();
                    }
                    if (Session["ans2"] != null)
                    {
                        ans[2] = Session["ans2"].ToString();
                    }

                    ans[ctr - 1] = usrInputField.Text;
                    ctr++;


                    usrInputField.Text = "";
                    string quest = giveInf(ctr);
                    questionLabel.Text = quest;
                    Session["num"] = ctr.ToString();

                    Session["ans0"] = ans[0];
                    Session["ans1"] = ans[1];
                    Session["ans2"] = ans[2];

                    if (ctr == 2)
                    {
                        input1.Text = ans[0];
                    }
                    else if (ctr == 3)
                    {
                        input2.Text = ans[1];
                    }

                    usrInputField.Focus();
                }
                else
                {
                    usrInputField.Text = "PLS TRY AGAIN";
                }
            }
            else if (ctr == 3)
            {
                if (Session["ans0"] != null)
                {
                    ans[0] = Session["ans0"].ToString();
                }
                if (Session["ans1"] != null)
                {
                    ans[1] = Session["ans1"].ToString();
                }

                ans[ctr - 1] = usrInputField.Text;
                input3.Text = ans[2];

                usrInputField.Text = "";
                questionLabel.Text = "Thanks!  Hold tight while we crunch the data for you";
                goButton.Visible = false;
                usrInputField.Visible = false;
                //ScriptManager.RegisterStartupScript(Page, GetType(), "showLoading", "<script>showLoading()</script>", false);
                loading.Visible = true;

                //loading.Visible = true;

                getAzureData(ans);
            }
        }
        public void getAzureData(string [] ansArr)
        {
            ////// FIX ME // - go here for input / output testing https://studio.azureml.net/Home/ViewWorkspaceCached/d5f6224965da42528c837dc03a23315e#Workspaces/WebServiceGroups/WebServiceGroup/60f243868d0c4e9a8448ff9d3ac58547/dashboard ///// 
            hops = Int32.Parse(ansArr[0]);
            malt = Int32.Parse(ansArr[2]);
            yeast = Int32.Parse(ansArr[1]);
            InvokeRequestResponseService(hops,malt,yeast).Wait();

            resultStr.Trim();
            string ans = resultStr.Substring(resultStr.IndexOf("Values: "), resultStr.IndexOf(","));

            //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Data is being parsed by Azure! (but not really)');", true);
            //Random rnd = new Random();
            double outFl = 4.2;
            //fdouble outFl = (float)(output / 100);
            usrInputField.Text = outFl.ToString();
            Double.TryParse(ans, out outFl);
            outFl = outFl / 100;
            //loading.Visible = false;
            ScriptManager.RegisterStartupScript(Page, GetType(), "hideLoading", "<script>hideLoading()</script>", false);

            if (outFl < 0.6)
            {
                outLabel.Text = "Hope you like PBR :)";
            }
            else if (outFl < 0.8 && outFl > 0.6)
            {
                outLabel.Text = "Eh, not a bad combo";
            }
            else if (outFl > 0.8 && outFl < 0.9)
            {
                outLabel.Text = "Great Combo!";
            }
            else if (outFl > 0.9)
                outLabel.Text = "Perfect Combo!";

        }

        /// <summary>
        /// Stuffs!! ////////////////////////
        /// </summary>
        /// <returns></returns>
        public static async Task InvokeRequestResponseService(int hop, int yeasts, int malts)
        {

            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {

                    Inputs = new Dictionary<string, StringTable>() {
                        {
                            "input1",
                            new StringTable()
                            {   
                                // FIX ME // - need to perhaps fill in other values as well, or change the neural net to not require them
                                //ColumnNames = new string[] {"Beer", "Bitter", "Sweet", "Salty", "Sour", "Alc%", "Mouth feel", "Head size", "Smell", "SRM", "Filtered", "Dryness", "Crispness", "Viscosity", "Serving Temp degrees F", "Nose Strength", "Brew Time", "Hops", "malt", "yeast", "Final Results"},
                                //Values = new string[,] {  { "value", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" },  { "value", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" },  }
                                ColumnNames = new string[] {"Beer", "Bitter", "Sweet", "Salty", "Sour", "Alc%", "Mouth feel", "Head size", "Smell", "SRM", "Filtered", "Dryness", "Crispness", "Viscosity", "Serving Temp degrees F", "Nose Strength", "Brew Time", "Hops", "malt", "yeast", "Final Results"},
                                Values = new string[,] {  { "value", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", hop.ToString(), yeasts.ToString(), malts.ToString(), "0" },  { "value", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", hop.ToString(), yeasts.ToString(), malts.ToString(), "0" },  }


                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };
                const string apiKey = "EuXaGm9VMcpxx/P9wermqIhhP7aKRdrTdx6EnthhDCRy1VZVJDf2hUyIJzpAKYSRfJgOq/mwFo9XDhtvAHUmtQ=="; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/d5f6224965da42528c837dc03a23315e/services/70abb8fd999c4cbfb95364f643b55386/execute?api-version=2.0&details=true");

                // WARNING: The 'await' statement below can result in a deadlock if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false) so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)

                // FIX ME // -> note: this never recieves a response, probably a simple fix.
                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Result: {0}", result);
                    resultStr = result;
                }
                else
                {
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp, which are useful for debugging the failure
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                }
            }
        }


    }

}