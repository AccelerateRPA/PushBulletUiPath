using System.Activities;
using System.ComponentModel;
using PushbulletSharp;
using PushbulletSharp.Models.Requests;
using PushbulletSharp.Models.Responses;
using System.Linq;


namespace PushBullet.Workflow.Activities
{
    public class NotifyASpecificDevice : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        [Description("The body of the notification to be sent")]
        public InArgument<string> Message { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("The title of the notification to be sent")]
        public InArgument<string> Title { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("Your PushBullet Account APIKey. This can be found in your PushBullet settings: https://www.pushbullet.com/#settings")]
        public InArgument<string> APIKey { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("Your PushBullet Device nickname. This can be found in your PushBullet settings: https://www.pushbullet.com/#devices")]
        public InArgument<string> DeviceNickname { get; set; }

        ///[Category("Output")]
        ///[Description("API Json response, use for exception identifcation")]
        ///public OutArgument<PushResponse> Response { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var messageBody = Message.Get(context);
            var messageTitle = Title.Get(context);
            var aPIKey = APIKey.Get(context);
            var identity = DeviceNickname.Get(context);

            PushbulletClient client = new PushbulletClient(aPIKey);
            var currentUserInformation = client.CurrentUsersInformation();

            var devices = client.CurrentUsersDevices();
            var device = devices.Devices.Where(o => o.Nickname == identity).FirstOrDefault();

            PushNoteRequest request = new PushNoteRequest
            {
                DeviceIden = device.Iden,
                Title = messageTitle,
                Body = messageBody
            };
            PushResponse response = client.PushNote(request);
            ///Response.Set(context, (response));
            ///.ToJson().ToString()
        }

    }
}