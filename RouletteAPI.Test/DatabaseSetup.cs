using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;

namespace RouletteAPI.Tests
{
    [SetUpFixture]
    public class DatabaseSetup
    {
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            // Set the path to SQLCMD executable
            string sqlCmdPath = @"C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\170\Tools\Binn\sqlcmd.exe"; // Example path, adjust as needed

            // Set the path to your SQL solution file
            string solutionFilePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\TestDbScript\", "CreateTestDatabase.sql");

            // Create process info
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = sqlCmdPath,
                Arguments = $"-S . -d RouletteAppTest -i {solutionFilePath}",
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            // Execute the SQLCMD process
            using (Process process = new Process())
            {
                process.StartInfo = psi;
                process.Start();
                process.WaitForExit();

                // Check for errors
                string errorOutput = process.StandardError.ReadToEnd();
                if (!string.IsNullOrEmpty(errorOutput))
                {
                    // Handle errors
                    Assert.Fail($"SQLCMD execution failed with error: {errorOutput}");
                }

                // Read output
                string output = process.StandardOutput.ReadToEnd();
            }
        }
    }
}
