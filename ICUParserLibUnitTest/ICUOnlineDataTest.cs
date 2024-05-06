// <copyright file="ICUOnlineDataTest.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace ICUParserLibUnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;
    using ICUParserLib;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests <see cref="ICUParser"/>.
    /// </summary>
    public partial class ICUTest
    {
        /// <summary>
        /// Validate the ICU lib data with the online data from
        /// http://www.unicode.org/cldr/charts/latest/supplemental/language_plural_rules.html .
        /// </summary>
        [TestMethod]
        public void TestOnlineDataValidationTest()
        {
            // Set up the PowerShell object.
            InitialSessionState initial = InitialSessionState.CreateDefault();
            initial.ApartmentState = System.Threading.ApartmentState.STA;
            initial.ThreadOptions = PSThreadOptions.UseCurrentThread;

            string scriptRoot = Path.GetFullPath(Path.Combine(this.TestContext.TestRunDirectory, @"..\..\ICUParserLib"));
            string scriptFile = Path.Combine(scriptRoot, "parseOnlineData.ps1");

            // Setup test data storage.
            Dictionary<string, string> testData = new Dictionary<string, string>();

            using (PowerShell powershell = PowerShell.Create(initial))
            {
                // Define environment.
                powershell.Runspace.SessionStateProxy.SetVariable("scriptRoot", scriptRoot);

                // Read the script file and pass it on to ps.
                string psScript = File.ReadAllText(scriptFile);
                powershell.AddScript(psScript);

                // Execute the script.
                try
                {
                    // Execute.
                    // Run the commands of the pipeline synchronously.
                    // 'results' is an array of objects left in the processing pipeline.
                    Collection<PSObject> results = powershell.Invoke();

                    // Log all pipeline objects.
                    foreach (PSObject result in results)
                    {
                        // Is object a string?
                        if (result.BaseObject is string msg)
                        {
                            // Get data set stats.
                            if (msg.StartsWith("DataSet="))
                            {
                                testData.Add("DataSet", msg);
                            }
                            else // Get non matching ci data.
                            if (msg.StartsWith("\nnon-matching CultureInfo for online data:"))
                            {
                                testData.Add("ciInfo", msg);
                            }
                        }
                    }

                    // Get warnings.
                    PSDataCollection<WarningRecord> warnings = powershell.Streams.Warning;
                    string warningText = string.Join($"{Environment.NewLine}", warnings);
                    if (!string.IsNullOrEmpty(warningText))
                    {
                        testData.Add("Warning", warningText);
                    }

                    // Get errors.
                    PSDataCollection<ErrorRecord> errors = powershell.Streams.Error;
                    string errorText = string.Join($"{Environment.NewLine}", errors);
                    if (!string.IsNullOrEmpty(errorText))
                    {
                        testData.Add("Error", errorText);
                    }
                }
                catch (Exception e)
                {
                    // Get exceptions.
                    if (!string.IsNullOrEmpty(e.Message))
                    {
                        testData.Add("Exception", e.Message);
                    }
                }
            }

            // Assert on result data.
            if (testData.ContainsKey("DataSet"))
            {
                Assert.AreEqual("DataSet=206 Languages: af,ak,sq,am,blo,ar,an,hy,as,ast,asa,az,bal,bm,bn,eu,be,bem,bez,bho,brx,bs,br,bg,my,yue,ca,ceb,tzm,ckb,ce,chr,cgg,zh,ksh,kw,hr,cs,da,dv,doi,nl,dz,en,eo,et,pt_PT,ee,fo,fil,fi,fr,fur,ff,gl,lg,ka,de,el,gu,ha,haw,he,hi,hnj,hu,is,io,ig,smn,id,ia,iu,ga,it,ja,jv,kaj,kea,kab,kkj,kl,kn,ks,kk,km,ko,ses,ku,ky,lkt,lag,lo,lv,lij,ln,lt,jbo,dsb,smj,lb,mk,jmc,kde,mg,ms,ml,mt,gv,mr,mas,mgo,mn,naq,ne,nnh,jgo,pcm,nd,se,nso,no,nb,nn,ny,nyn,or,om,osa,os,pap,ps,fa,pl,pt,prg,pa,ro,rm,rof,ru,rwk,ssy,saq,sg,sat,sc,gd,seh,sr,ksb,sn,ii,scn,sd,si,sms,sk,sl,xog,so,nr,sdh,sma,st,es,su,sw,ss,sv,gsw,syr,shi,ta,te,teo,th,bo,tig,ti,tpi,to,ts,tn,tr,tk,kcg,uk,hsb,ur,ug,uz,ve,vec,vi,vo,vun,wa,wae,cy,fy,wo,xh,sah,yo,zu,", testData["DataSet"]);
            }
            else
            {
                Assert.Fail("DataSet stats could not be validated.");
            }

            if (testData.ContainsKey("ciInfo"))
            {
                Assert.AreEqual("\nnon-matching CultureInfo for online data:\n'Unknown Language (blo)' -ne 'Anii'\n'Bamanankan' -ne 'Bambara'\n'Unknown Language (yue)' -ne 'Cantonese'\n'Portuguese (Portugal)' -ne 'European Portuguese'\n'Unknown Language (hnj)' -ne 'Hmong Njua'\n'Sami (Inari)' -ne 'Inari Sami'\n'Unknown Language (jbo)' -ne 'Lojban'\n'Sami (Lule)' -ne 'Lule Sami'\n'Unknown Language (osa)' -ne 'Osage'\n'Yi' -ne 'Sichuan Yi'\n'Sami (Skolt)' -ne 'Skolt Sami'\n'Sami (Southern)' -ne 'Southern Sami'\n'Unknown Language (tpi)' -ne 'Tok Pisin'\n'Unknown Language (vec)' -ne 'Venetian'\n'Sakha' -ne 'Yakut'", testData["ciInfo"]);
            }
            else
            {
                Assert.Fail("Non matching CI data could not be validated.");
            }

            // No Warnings expected.
            if (testData.ContainsKey("Warning"))
            {
                Assert.Fail(testData["Warning"]);
            }

            // No Errors expected.
            if (testData.ContainsKey("Error"))
            {
                Assert.Fail(testData["Error"]);
            }

            // No Exceptions expected.
            if (testData.ContainsKey("Exception"))
            {
                Assert.Fail(testData["Exception"]);
            }
        }
    }
}
