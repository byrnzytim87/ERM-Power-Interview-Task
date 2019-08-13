using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using Newtonsoft;


namespace ERMPowerClientApp
{
    public class Options
    {
        [Option('f', Required = true, HelpText = "The file path to the required CSV file")]
        public string FilePath { get; set; }

        [Option('t', Required = true, HelpText = "The file type - LP or TOU. e.g. -t LP")]
        public string FileType { get; set; }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            string json = string.Empty;

            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       if (o.FilePath.Length != 0)
                       {
                           var csv = new List<string[]>();

                           // Load file
                           var csvLines = System.IO.File.ReadAllLines(o.FilePath);

                           // Convert CSV data to JSON
                           foreach (string line in csvLines)
                           {
                               csv.Add(line.Split(','));
                               // ASSUMPTION: There won't be any commas in the test data. Should
                               // use an appriate library to handle this, if so.
                           }

                           // Remove header row in preparation for serialization, if included
                           // ASSUMPTION: That the file will always contain a header row with the same headers for the file type
                           csv.RemoveAt(0);

                           if (o.FileType.ToUpper().Equals("LP"))
                           {
                               var lPList = new List<LP>();

                               foreach (var element in csv)
                               {
                                   lPList.Add(new LP()
                                   {
                                       MeterPointCode = element[0],
                                       SerialNumber = int.Parse(element[1]),
                                       PlantCode = element[2],
                                       Date = DateTime.Parse(element[3]),
                                       DataType = element[4],
                                       DataValue = float.Parse(element[5]),
                                       Units = element[6],
                                       Status = element[7]
                                   });
                               }

                               json = Newtonsoft.Json.JsonConvert.SerializeObject(lPList);
                           }

                           if (o.FileType.ToUpper().Equals("TOU"))
                           {
                               var tOUList = new List<TOU>();

                               foreach (var element in csv)
                               {
                                   tOUList.Add(new TOU()
                                   {
                                       MeterPointCode = element[0],
                                       SerialNumber = int.Parse(element[1]),
                                       PlantCode = element[2],
                                       Date = DateTime.Parse(element[3]),
                                       Quality = element[4],
                                       Stream = element[5],
                                       DataType = element[6],
                                       Energy = float.Parse(element[7]),
                                       Units = element[8]
                                   });
                               }

                               json = Newtonsoft.Json.JsonConvert.SerializeObject(tOUList);
                           }
                       }
                       else
                       {
                           Console.WriteLine("File is empty");
                       }
                   });

            // Construct HTTP Post 
            // Send to AWS
            // Receive response
            var response = await SendRequest(json, "https://pqfny1q7g8.execute-api.ap-southeast-2.amazonaws.com/Test/erm");


            Console.ReadKey();

        }

        public static async Task<HttpResponseMessage> SendRequest(string json, string url)
        {
            // Instantiate once, should reuse HttpClient rather than dispose
            var client = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();

            if (json != string.Empty)
            {
                try
                {
                    response = await client.PostAsync(
                        url,
                        new StringContent(json, Encoding.UTF8, "application/json"));
                    Console.WriteLine(response);
                    Console.WriteLine(response.RequestMessage);
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return response;
        }


    }
}
