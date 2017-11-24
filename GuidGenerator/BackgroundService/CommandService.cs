using System;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.VoiceCommands;

namespace GuidGenerator.BackgroundService
{
    public sealed class CommandService : IBackgroundTask
    {
        private BackgroundTaskDeferral _serviceDeferral;
        VoiceCommandServiceConnection _voiceServiceConnection;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _serviceDeferral = taskInstance.GetDeferral();

            var triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;

            if (triggerDetails != null && triggerDetails.Name == "GuidGeneratorCommandService")
            {
                _voiceServiceConnection = VoiceCommandServiceConnection.FromAppServiceTriggerDetails(triggerDetails);
                _voiceServiceConnection.VoiceCommandCompleted += VoiceCommand_Completed;
                var voiceCommand = await _voiceServiceConnection.GetVoiceCommandAsync();
                var message = new VoiceCommandUserMessage();
                VoiceCommandResponse response;

                switch (voiceCommand.CommandName)
                {
                    case "newGuid":
                    case "generateGuid":
                        var guid = Guid.NewGuid();
                        message.DisplayMessage = $"Here you go: {guid.ToString()}";
                        message.SpokenMessage = "Here you go!";
                        response = VoiceCommandResponse.CreateResponse(message);
                        await _voiceServiceConnection.ReportSuccessAsync(response);
                        break;
                    default:
                        message.SpokenMessage = "Launching GUID Generator";
                        response = VoiceCommandResponse.CreateResponse(message);
                        await _voiceServiceConnection.RequestAppLaunchAsync(response);
                        break;
                }
            }
        }

        private void VoiceCommand_Completed(VoiceCommandServiceConnection sender, VoiceCommandCompletedEventArgs args) =>
            _serviceDeferral?.Complete();
    }
}
