using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.Devices;
using Microsoft.VisualBasic.MyServices;
using PV_Key_Scraper_and_Checker.My;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace PV_Key_Scraper_and_Checker
{
	internal static class Module1
	{
		private static int counter;

		private static int lage;

		private static string addys;

		private static Queue<string> pkeyz;

		private static List<string> @checked;

		private static List<string> duplicates;

		private static int errors;

		private static int page;

		private static string str;

		static Module1()
		{
			Module1.addys = "addys.txt";
			Module1.pkeyz = new Queue<string>();
			Module1.@checked = new List<string>();
			Module1.duplicates = new List<string>();
		}

		private static string GenMask(string Mask)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Random random = new Random();
			int length = checked(Mask.Length - 1);
			for (int i = 0; i <= length; i = checked(i + 1))
			{
				if (Operators.CompareString(Conversions.ToString(Mask[i]), "@", false) != 0)
				{
					stringBuilder.Append(Mask[i]);
				}
				else
				{
					stringBuilder.Append(random.Next(0, 9));
				}
			}
			return stringBuilder.ToString();
		}

		[STAThread]
		public static void Main()
		{
			IEnumerator enumerator = null;
			IEnumerator enumerator1 = null;
			ConsoleKeyInfo consoleKeyInfo;
			while (true)
			{
				try
				{
					Module1.str = Module1.GenMask("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
					HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Concat("https://lbc.cryptoguru.org/dio/", Module1.str.ToString()));
					httpWebRequest.Proxy = null;
					httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";
					httpWebRequest.Timeout = 5000;
					StreamReader streamReader = new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream());
					string end = streamReader.ReadToEnd();
					streamReader.Close();
					MatchCollection matchCollections = Regex.Matches(end, "</span> <a href=\"https://blockchain.info/address/(.*?)\">(.*?)</a>");
					MatchCollection matchCollections1 = Regex.Matches(end, "<span id=\"(.*?)\">");
					try
					{
						enumerator = matchCollections1.GetEnumerator();
						while (enumerator.MoveNext())
						{
							Match current = (Match)enumerator.Current;
							Module1.pkeyz.Enqueue(current.Groups[1].Value);
						}
					}
					finally
					{
						if (enumerator is IDisposable)
						{
							(enumerator as IDisposable).Dispose();
						}
					}
					try
					{
						enumerator1 = matchCollections.GetEnumerator();
						while (enumerator1.MoveNext())
						{
							Match match = (Match)enumerator1.Current;
							string[] str = new string[] { "Wallets checked: ", null, null, null, null, null, null, null };
							int count = Module1.@checked.Count;
							str[1] = count.ToString();
							str[2] = " | Duplicates: ";
							count = Module1.duplicates.Count;
							str[3] = count.ToString();
							str[4] = " | Errors: ";
							str[5] = Module1.errors.ToString();
							str[6] = " | Current page: ";
							str[7] = Module1.str.ToString();
							Console.Title = string.Concat(str);
							if (!Module1.@checked.Contains(match.Groups[1].Value))
							{
								Module1.@checked.Add(match.Groups[1].Value);
								string str1 = Module1.pkeyz.Dequeue();
								HttpWebRequest httpWebRequest1 = (HttpWebRequest)WebRequest.Create(string.Concat("https://blockchain.info/address/", match.Groups[1].Value));
								httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";
								HttpWebResponse response = (HttpWebResponse)httpWebRequest1.GetResponse();
								string end1 = (new StreamReader(response.GetResponseStream())).ReadToEnd();
								streamReader.Close();
								Match match1 = Regex.Match(end1, "<td id=\"final_balance\"><font color=\"green\"><span data-c=\"(.*?)\">(.*?) BTC</span></font></td>");
								Module1.counter = checked(Module1.counter + 1);
								if (Operators.CompareString(match1.Groups[2].Value, "0", false) != 0)
								{
									Console.ForegroundColor = ConsoleColor.Green;
									Console.WriteLine(string.Concat(new string[] { "[✓] ", match.Groups[2].Value, " pKey: ", str1, " Balance: ", match1.Groups[1].Value }));
									Console.WriteLine("Found balance!");
									consoleKeyInfo = Console.ReadKey();
									return;
								}
								else
								{
									Console.ForegroundColor = ConsoleColor.Red;
									Console.WriteLine(string.Concat(new string[] { "[x] ", match.Groups[2].Value, " pKey: ", str1, " Balance: ", match1.Groups[1].Value }));
									StreamWriter streamWriter = MyProject.Computer.FileSystem.OpenTextFileWriter("empty_wallets.txt", true);
									streamWriter.WriteLine(match.Groups[1].Value);
									streamWriter.Close();
									StreamWriter streamWriter1 = MyProject.Computer.FileSystem.OpenTextFileWriter("empty_wallets_with_pkey.txt", true);
									streamWriter1.WriteLine(string.Concat(match.Groups[1].Value, " | ", str1));
									streamWriter1.Close();
								}
							}
							else
							{
								Console.ForegroundColor = ConsoleColor.Yellow;
								Console.WriteLine("Wallet already used!");
								Module1.duplicates.Add(match.Groups[1].Value);
								
							}
						}
					}
					finally
					{
						if (enumerator1 is IDisposable)
						{
							(enumerator1 as IDisposable).Dispose();
						}
					}

				}
				catch (Exception exception)
				{
					ProjectData.SetProjectError(exception);
					Console.ForegroundColor = ConsoleColor.DarkMagenta;
					Console.WriteLine("Connection error!");
					Module1.errors = checked(Module1.errors + 1);
					ProjectData.ClearProjectError();
				}
                Console.WriteLine("Found balance!");
                consoleKeyInfo = Console.ReadKey();
            }
		}
	}
}