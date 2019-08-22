
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AsterNET.FastAGI;
using AsterNET.FastAGI.Command;
using AsterNET.Manager;
using AsterNET.Manager.Action;

namespace MegaSolucao.Negocio.URAs
{
    public class MegaIVR : AGIScript
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

        // const string DEV_HOST = "192.168.15.9";
        const int ASTERISK_PORT = 5038;
        const string ASTERISK_HOST = "192.168.15.240";
        const string ASTERISK_LOGINNAME = "snep";
        const string ASTERISK_LOGINPWD = "sneppass";

        #region Speech Test

        //public static async Task RecognizeSpeechAsync()
        //{
        //    // Creates an instance of a speech config with specified subscription key and service region.
        //    // Replace with your own subscription key and service region (e.g., "westus").
        //    var config = SpeechConfig.FromSubscription("YourSubscriptionKey", "YourServiceRegion");

        //    // Creates a speech recognizer.
        //    using (var recognizer = new SpeechRecognizer(config))
        //    {
        //        Console.WriteLine("Say something...");

        //        // Starts speech recognition, and returns after a single utterance is recognized. The end of a
        //        // single utterance is determined by listening for silence at the end or until a maximum of 15
        //        // seconds of audio is processed.  The task returns the recognition text as result. 
        //        // Note: Since RecognizeOnceAsync() returns only a single utterance, it is suitable only for single
        //        // shot recognition like command or query. 
        //        // For long-running multi-utterance recognition, use StartContinuousRecognitionAsync() instead.
        //        var result = await recognizer.RecognizeOnceAsync();

        //        // Checks result.
        //        if (result.Reason == ResultReason.RecognizedSpeech)
        //        {
        //            Console.WriteLine($"We recognized: {result.Text}");
        //        }
        //        else if (result.Reason == ResultReason.NoMatch)
        //        {
        //            Console.WriteLine($"NOMATCH: Speech could not be recognized.");
        //        }
        //        else if (result.Reason == ResultReason.Canceled)
        //        {
        //            var cancellation = CancellationDetails.FromResult(result);
        //            Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

        //            if (cancellation.Reason == CancellationReason.Error)
        //            {
        //                Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
        //                Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
        //                Console.WriteLine($"CANCELED: Did you update the subscription info?");
        //            }
        //        }
        //    }
        //}

        #endregion

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
                    Thread.Sleep(2000);
                    

                    key = StreamFile("ARQUIVO DE AUDIO COREM ATENDEDOR", escapeKeys);
                    welcome = false;
                    submenu = 0;

                    
                    
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
                                newKey = StreamFile("ARQUIVO DE AUDIO OP1-SUB", escapeKeys);
                                submenu = 1;
                                break;
                            case '2':
                                newKey = StreamFile("press-2", escapeKeys);
                                submenu = 2;
                                break;
                            case '3':
                                newKey = StreamFile("press-3", escapeKeys);
                                submenu = 3;
                                break;
                            default:
                                newKey = StreamFile("bad", escapeKeys);
                                if (newKey == '\0')
                                    newKey = StreamFile("digit", escapeKeys);
                                break;
                        }
                        break;


                    case 1:
                        switch (key)
                        {
                            case '1':
                                var audio = RecordFile("aaa", "wav", escapeKeys, 3000);
                                


                                newKey = StreamFile("ARQUIVO DE AUDIO OP1-SUB1-GOIANIA", escapeKeys);
                                submenu = 11;
                                break;
                            case '2':
                                newKey = StreamFile("ARQUIVO DE AUDIO OP1-SUB2-ANAPOLIS", escapeKeys);
                                submenu = 12;
                                break;
                            case '3':
                                newKey = StreamFile("ARQUIVO DE AUDIO OP1-SUB3-VALPARAISO", escapeKeys);
                                submenu = 13;
                                break;
                            default:
                                newKey = StreamFile("bad", escapeKeys);
                                if (newKey == '\0')
                                    newKey = StreamFile("digit", escapeKeys);
                                break;
                        }
                        break;

                    case 11:
                        switch (key)
                        {
                            case '7':
                                submenu = 1;
                                newKey = '1';
                                break;
                            case '8':
                                submenu = 0;
                                welcome = true;
                                break;
                        }
                        break;
                    case 12:
                        switch (key)
                        {
                            case '7':
                                submenu = 1;
                                newKey = '2';
                                break;
                            case '8':
                                submenu = 0;
                                welcome = true;
                                break;
                            default:
                                submenu = 1;
                                newKey = '2';
                                break;
                        }
                        break;
                    case 13:
                        switch (key)
                        {
                            case '7':
                                submenu = 1;
                                newKey = '3';
                                break;
                            case '8':
                                submenu = 0;
                                welcome = true;
                                break;
                        }
                        break;

                    case 2:
                        switch (key)
                        {
                            case '1':
                                
                                submenu = 21;
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