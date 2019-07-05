using System.Activities;
using System.ComponentModel;
using System.Linq;
using PushbulletSharp;
using PushbulletSharp.Models.Requests;
using PushbulletSharp.Models.Responses;


namespace PushBullet.Workflow.Activities
{
    public class PushLinkToASpecificDevice : CodeActivity
    {
        [Category("Input")]
        [Description("The body of the notification to be sent")]
        public InArgument<string> Message { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("The title of the notification to be sent")]
        public InArgument<string> Title { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("The URL of link you wish to be sent")]
        public InArgument<string> URL { get; set; }

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
            var uRL = URL.Get(context);
            var identity = DeviceNickname.Get(context);

            PushbulletClient client = new PushbulletClient(aPIKey);
            var currentUserInformation = client.CurrentUsersInformation();

            var devices = client.CurrentUsersDevices();
            var device = devices.Devices.Where(o => o.Nickname == identity).FirstOrDefault();

            PushLinkRequest request = new PushLinkRequest()
            {
                DeviceIden = device.Iden,
                Email = currentUserInformation.Email,
                Title = messageTitle,
                Body = messageBody,
                Url = uRL
            };

            PushResponse response = client.PushLink(request);
            ///Response.Set(context, (response));
            ///.ToJson().ToString()
        }
    }
}