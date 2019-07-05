using System.IO;
using System.Linq;
using System.Activities;
using System.ComponentModel;
using PushbulletSharp;
using PushbulletSharp.Models.Requests;
using PushbulletSharp.Models.Responses;
using PushbulletSharp.Models.Requests.Ephemerals;

namespace PushBullet.Workflow.Activities
{
    public class SendSMSThroughDevice : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        [Description("The body of the notification to be sent")]
        public InArgument<string> Message { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("Your PushBullet Account APIKey. This can be found in your PushBullet settings: https://www.pushbullet.com/#settings")]
        public InArgument<string> APIKey { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("The key input to all devices for end to end encryption. See here for more details: https://docs.pushbullet.com/#end-to-end-encryption")]
        public InArgument<string> EncryptionKey { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("The PushBullet Device nickname for the android phone the message will be sent via. This can be found in your PushBullet settings: https://www.pushbullet.com/#devices")]
        public InArgument<string> DeviceNickname { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("The phone number(s) to send this message to. The country code must be used e.g. 447123123123.")]
        public InArgument<string> ToPhoneNumber { get; set; }

        ///[Category("Output")]
        ///[Description("API Json response, use for exception identifcation")]
        ///public OutArgument<PushResponse> Response { get; set; }

        protected override void Execute(CodeActivityContext context)
        {


            var messageBody = Message.Get(context);
            var aPIKey = APIKey.Get(context);
            var identity = DeviceNickname.Get(context);
            var encryptionKey = EncryptionKey.Get(context);
            var phoneNumber = ToPhoneNumber.Get(context);

            PushbulletClient client = new PushbulletClient(aPIKey, encryptionKey);
            var currentUserInformation = client.CurrentUsersInformation();
            var devices = client.CurrentUsersDevices();
            var smsDevice = devices.Devices.Where(o => o.Nickname == identity).FirstOrDefault();

            SMSEphemeral smsRequest = new SMSEphemeral()
            {
                ConversationIden = phoneNumber,
                Message = messageBody,
                TargetDeviceIden = smsDevice.Iden,
                PackageName = "com.pushbullet.android"
            };
            var response = client.PushEphemeral(smsRequest, true);

        }
    }
}
