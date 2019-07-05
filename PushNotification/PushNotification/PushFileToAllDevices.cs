using System.IO;
using System.Linq;
using System.Activities;
using System.ComponentModel;
using PushbulletSharp;
using PushbulletSharp.Models.Requests;
using PushbulletSharp.Models.Responses;

namespace PushBullet.Workflow.Activities
{
    public class PushFileToAllDevices : CodeActivity
    {
        [Category("Input")]
        [Description("The body of the notification to be sent")]
        public InArgument<string> Message { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("The full file path of the file you wish to be sent")]
        public InArgument<string> FilePath { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("The desired name for the file after it has been sent (including file extension)")]
        public InArgument<string> FileName { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("The file extension as a string")]
        public InArgument<string> FileExtension { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("Your PushBullet Account APIKey. This can be found in your PushBullet settings: https://www.pushbullet.com/#settings")]
        public InArgument<string> APIKey { get; set; }

        ///[Category("Output")]
        ///[Description("API Json response, use for exception identifcation")]
        ///public OutArgument<PushResponse> Response { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var messageBody = Message.Get(context);
            var aPIKey = APIKey.Get(context);
            var filePath = FilePath.Get(context);
            var fileName = FileName.Get(context);
            var fileExtension = FileExtension.Get(context);

            if (fileExtension.Contains("."))
            {
                fileExtension.Replace(".", "");
            }

            PushbulletClient client = new PushbulletClient(aPIKey);
            var currentUserInformation = client.CurrentUsersInformation();

            using (var fileStream = new FileStream(@filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                PushFileRequest request = new PushFileRequest()
                {
                    Email = currentUserInformation.Email,
                    FileName = fileName,
                    FileType = fileExtension,
                    FileStream = fileStream,
                    Body = messageBody
                };

                PushResponse response = client.PushFile(request);
            }
        }
    }
}