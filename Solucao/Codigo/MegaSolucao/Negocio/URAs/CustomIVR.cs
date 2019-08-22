using System;
using System.Threading;
using AsterNET;
using AsterNET.FastAGI;
using AsterNET.FastAGI.Command;
using AsterNET.Manager;
using AsterNET.Manager.Action;

namespace MegaSolucao.Negocio.URAs
{
	public class CustomIVR : AGIScript
	{
		private string escapeKeys = "0123456789*#";
        /*
		 * Call -> play "wellcome" -> wait digit 5 seconds -> press 1 -> play "press-1" -> wait digit -> press * ----------------------------\
		 *               ^                    ^                                                 ^     -> press 4 -> play "press-4" -------\  |
		 *               |                    |                                                 |     -> press any -> play "bad" "digit" -+  |
		 *               |                    |                                                 \-----------------------------------------/  |
		 *               |                    |                                                       -> press # or timeout -\               |
		 *               |                    |                                                                              |               |
		 *               |                    |            -> press # or timeout -> play "goodbye" -> Hangup                 |               |
		 *               |                    |                                          ^                                   |               |
		 *               |                    |                                          \-----------------------------------+               |
		 *               |                    |                                                                              |               |
		 *               |                    |                                                                              |               |
		 *               |                    |           -> press 2 -> play "press-1" -> wait digit  -> press # or timeout -/               |
		 *               |                    |                                                 ^     -> press * ----------------------------+
		 *               |                    |                                                 |     -> press 4 -> play "press-4" -------\  |
		 *               |                    |                                                 |     -> press any -> play "bad" "digit" -+  |
		 *               |                    |                                                 \-----------------------------------------/  |
		 *               |                    |                                                                                              |
		 *               |                    |            -> press other -> play "bad" "digit" -\                                           |
		 *               |                    \--------------------------------------------------/                                           |
		 *               \-------------------------------------------------------------------------------------------------------------------/
		 */

        //onst string DEV_HOST = "192.168.15.9";
        const int ASTERISK_PORT = 5038;
        const string ASTERISK_HOST = "192.168.15.240";
        const string ASTERISK_LOGINNAME = "snep";
        const string ASTERISK_LOGINPWD = "sneppass";

        

        public override void Service(AGIRequest request, AGIChannel channel)
		{
            var quemLigou = string.Empty;
			Answer();
			int submenu = 0;
			char key = '\0';
			bool welcome = true;

			while (true)
			{
				if (welcome)
				{
                    Thread.Sleep(5000);

					key = StreamFile("welcome", escapeKeys);
					welcome = false;
					submenu = 0;

                    quemLigou = request.Request["callerid"].ToString();
                    Thread.Sleep(2000);

                    Persistencia.FilaPraLigar.Add(quemLigou);
                    Hangup();

                    return;

                    //SetVariable("callerid", "9002");
                    //var callerId = GetVariable("agi_callerid"); //channel.SendCommand(new GetVariableCommand("callerid"));
                    //SetExtension("9002");
                    //return;
                    //Exec("9003");
				}
				if (key == '\0')
				{
					key = WaitForDigit(5000);
					if (key == '\0' || key == '#')
					{
						StreamFile("goodbye");
						break;
					}
				}
				char newKey = '\0';
				switch (submenu)
				{
					case 0:
						switch (key)
						{
							case '1':
								newKey = StreamFile("press-1", escapeKeys);
								submenu = 1;
								break;
							case '2':
								newKey = StreamFile("press-2", escapeKeys);
								submenu = 2;
								break;
							case '3':
								newKey = StreamFile("press-3", escapeKeys);
								break;
							default:
								newKey = StreamFile("bad", escapeKeys);
								if(newKey == '\0')
									newKey = StreamFile("digit", escapeKeys);
								break;
						}
						break;
					case 1:
						switch (key)
						{
							case '*':
								welcome = true;
								break;
							case '4':
								newKey = StreamFile("press-4", escapeKeys);
								break;
							default:
								newKey = StreamFile("bad", escapeKeys);
								if (newKey == '\0')
									newKey = StreamFile("digit", escapeKeys);
								break;
						}
						break;
					case 2:
						switch (key)
						{
							case '*':
								welcome = true;
								break;
							case '5':
								newKey = StreamFile("press-5", escapeKeys);
								break;
							default:
								newKey = StreamFile("bad", escapeKeys);
								if (newKey == '\0')
									newKey = StreamFile("digit", escapeKeys);
								break;
						}
						break;
				}
				key = newKey;
			}
            
			Hangup();
        }
	}
}
