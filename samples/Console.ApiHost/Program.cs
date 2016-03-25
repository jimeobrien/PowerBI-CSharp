﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.PowerBI.Api.Beta;
using Microsoft.PowerBI.Api.Beta.Models;
using Microsoft.Rest;
using Console = System.Console;
using Microsoft.Threading;
using ApiHost.Models;
using System.IO;

namespace ApiHost
{
    class Program
    {
        static void Main(string[] args)
        {
            AsyncPump.Run(async delegate
            {
                await DemoAsync();
            });

            Console.ReadKey(true);
        }

        static async Task DemoAsync()
        {
            var resourceUri = "https://analysis.windows.net/powerbi/api";
            var authority = "https://login.windows-ppe.net/common/oauth2/authorize";
            var clientId = "f3ea3093-dec5-48ad-97e5-ab5f491a848e";
            var redirectUri = "https://login.live.com/oauth20_desktop.srf";

            //var tokenCache = new TokenCache();
            //var authContext = new AuthenticationContext(authority, tokenCache);
            //var authResult = authContext.AcquireToken(resourceUri, clientId, new Uri(redirectUri), PromptBehavior.Always);

            var credentials = new TokenCredentials("eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjF6bmJlNmV2ZWJPamg2TTNXR1E5X1ZmWXVJdyIsImtpZCI6IjF6bmJlNmV2ZWJPamg2TTNXR1E5X1ZmWXVJdyJ9.eyJhdWQiOiJodHRwczovL2FuYWx5c2lzLndpbmRvd3MtaW50Lm5ldC9wb3dlcmJpL2FwaSIsImlzcyI6Imh0dHBzOi8vc3RzLndpbmRvd3MtcHBlLm5ldC9mNjg2ZDQyNi04ZDE2LTQyZGItODFiNy1hYjU3OGUxMTBjY2QvIiwiaWF0IjoxNDU2NDMyMTkxLCJuYmYiOjE0NTY0MzIxOTEsImV4cCI6MTQ1NjQzNjA5MSwiYWNyIjoiMSIsImFtciI6WyJwd2QiLCJtZmEiXSwiYXBwaWQiOiI4NzFjMDEwZi01ZTYxLTRmYjEtODNhYy05ODYxMGE3ZTkxMTAiLCJhcHBpZGFjciI6IjIiLCJmYW1pbHlfbmFtZSI6IkJyZXphIiwiZ2l2ZW5fbmFtZSI6IldhbGxhY2UiLCJpcGFkZHIiOiIxMDQuNDAuMjMuMTgiLCJuYW1lIjoiV2FsbGFjZSBCcmV6YSIsIm9pZCI6IjBjNDVhNmNiLTU4MzQtNDE3ZS04ODFhLTg5MDNjNjQxN2FhZCIsIm9ucHJlbV9zaWQiOiJTLTEtNS0yMS0yMTI3NTIxMTg0LTE2MDQwMTI5MjAtMTg4NzkyNzUyNy02NzQ0NjUyIiwicHVpZCI6IjEwMDMzRkZGODAyRTM2MkUiLCJzY3AiOiJ1c2VyX2ltcGVyc29uYXRpb24iLCJzdWIiOiJ2T200WmRrbXUwNHJUcVRQaHZpUFdGT2RqU09zTkxfZHU4WmdsNjFUYmVrIiwidGlkIjoiZjY4NmQ0MjYtOGQxNi00MmRiLTgxYjctYWI1NzhlMTEwY2NkIiwidW5pcXVlX25hbWUiOiJ3YWJyZXphQG1pY3Jvc29mdC5jb20iLCJ1cG4iOiJ3YWJyZXphQG1pY3Jvc29mdC5jb20iLCJ2ZXIiOiIxLjAifQ.rVcwQk8CFrvI05h_Zw2VbBxvRwuWFWtqlRg4m15dTBW-ydTS2DoVFrvou23n5y7AzRmgPNArLgdjCeQAjcOujbjB_kaQV63DzGJZJUdgTJmfxXjgutLqLA_5MyU4rXhcHV8wRuklr-dyADLUjESQJILOsuPFwqlVfbF0IZ4gHW6Q6qjpBF0Cjhfw358IXJfXhzdlLVxzpNa3sXVZgZgEbzjX3VJQIMh-b2Xo7J90P0kzG1H196AZFRdLaCrlITLe3NjVnpZL7d27uTpQa7LB_g2sYAhMoNwHBoYKTS6o8iMMfxgYftE5HlW1slfAntWYcIrfOH2LlERknITeXG_M9g", "Bearer");
            var client = new PowerBIClient(credentials);
            client.BaseUri = new Uri("https://bipmdevcst3-redirect.analysis.windows-int.net/");

            var imports = await client.Imports.GetImportsAsync();

            Console.WriteLine();
            Console.WriteLine("Imports");
            Console.WriteLine("================================");
            foreach (var import in imports.Value)
            {
                Console.WriteLine("{0}", import.Name);
            }

            Console.WriteLine();
            Console.WriteLine("Import PBIX? (y/N)");
            var key = Console.ReadKey(true);
            if (key.KeyChar == 'Y')
            {
                using (var pbix = File.OpenRead(@"c:\users\wabreza\Desktop\progress.pbix"))
                {
                    await client.Imports.PostImportWithFileAsync(pbix, "Progress");
                }
            }

            return;


            var dashboards = await client.Dashboards.GetDashboardsAsync();

            Console.WriteLine();
            Console.WriteLine("Dashboards");
            Console.WriteLine("================================");
            foreach (var dashboard in dashboards.Value)
            {
                Console.WriteLine("{0}", dashboard.DisplayName);

                var tiles = await client.Dashboards.GetTilesByDashboardkeyAsync(dashboard.Id);
                foreach (var tile in tiles.Value)
                {
                    Console.WriteLine("-- {0} - {1}", tile.Title, tile.EmbedUrl);
                }
            }

            var reports = await client.Reports.GetReportsAsync();

            Console.WriteLine();
            Console.WriteLine("Reports");
            Console.WriteLine("================================");
            foreach (var report in reports.Value)
            {
                Console.WriteLine("{0} - {1}", report.Name, report.EmbedUrl);
            }

            var datasets = await client.Datasets.GetDatasetsAsync();

            Console.WriteLine();
            Console.WriteLine("Datasets");
            Console.WriteLine("================================");
            foreach (var dataset in datasets.Value)
            {
                Console.WriteLine("{0} ({1})", dataset.Name, dataset.Id);

                try
                {
                    var tables = await client.Datasets.GetTablesByDatasetkeyAsync(dataset.Id);
                    foreach (var table in tables.Value)
                    {
                        Console.WriteLine("-- {0}", table.Name);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("-- {0}", ex.Message);
                }

                Console.WriteLine();
            }

            var groups = await client.Groups.GetGroupsAsync();

            Console.WriteLine();
            Console.WriteLine("Groups");
            Console.WriteLine("================================");
            foreach (var group in groups.Value)
            {
                Console.WriteLine("{0}", group.Name);

                var groupDashboards = await client.Dashboards.GetDashboardsByGroupAsync(group.Id);
                Console.WriteLine();
                Console.WriteLine("Group Dashboards");
                Console.WriteLine("================================");

                foreach (var groupDashboard in groupDashboards.Value)
                {
                    Console.WriteLine("--{0}", groupDashboard.DisplayName);
                }
            }

            var datasetTables = new List<Table>
            {
                new Table("Users", new List<Column>()
                {
                    new Column {Name = "Id", DataType = "string"},
                    new Column {Name = "FirstName", DataType = "string"},
                    new Column {Name = "LastName", DataType = "string"},
                    new Column {Name = "Email", DataType = "string"}
                })
            };

            //var newDataset = new Dataset("Foobar - " + DateTime.Now.ToShortTimeString(), datasetTables);
            //var foo = await client.Datasets.PostDatasetAsync(newDataset) as Dataset;

            var datasetKey = "4f5cc321-0007-42ff-afea-d7b3926172be";
            var tableName = "Users";

            await client.Datasets.DeleteRowsByDatasetkeyAndTablenameAsync(datasetKey, tableName);

            var rows = new List<User>
            {
                new User{ Id = "1", FirstName= "Wallace", LastName = "Breza" },
                new User{ Id = "2", FirstName= "Jon", LastName = "Gallant" },
                new User{ Id = "3", FirstName= "Will", LastName = "Anderson" },
                new User{ Id = "4", FirstName= "Tony", LastName = "Ferrel" },
            };

            var request = new DatasetOperation<User>(rows);

            await client.Datasets.PostRowsByDatasetkeyAndTablenameAsync(datasetKey, tableName, request);
        }
    }
}