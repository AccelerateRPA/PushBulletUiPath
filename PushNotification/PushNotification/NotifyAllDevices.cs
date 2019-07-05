using System.Activities;
using System.ComponentModel;
using PushbulletSharp;
using PushbulletSharp.Models.Requests;
using PushbulletSharp.Models.Responses;


namespace PushBullet.Workflow.Activities
{
    public class NotifyAllDevices : CodeActivity
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

        ///[Category("Output")]
        ///[Description("API Json response, use for exception identifcation")]
        ///public OutArgument<PushResponse> Response { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var messageBody = Message.Get(context);
            var messageTitle = Title.Get(context);
            var aPIKey = APIKey.Get(context);

            PushbulletClient client = new PushbulletClient(aPIKey);
            var currentUserInformation = client.CurrentUsersInformation();

            PushNoteRequest request = new PushNoteRequest
            {
                Email = currentUserInformation.Email,
                Title = messageTitle,
                Body = messageBody
            };
            PushResponse response = client.PushNote(request);
            ///Response.Set(context, (response));
            ///.ToJson().ToString()
        }

    }
}