using System;
using System.IO;
using Dynamo.Extensions;
using Dynamo.Logging;
using SampleLinter.Rules;

namespace SampleLinter
{
    public class SampleLinter : LinterExtensionBase
    {
        public override string UniqueId => "44BAAD49-4750-47BE-AB60-61DD30962FAE"; //this is unique to this linter
        public override string Name => "Sample Linter";

        
        private SampleSliderInputLinterRule _sliderInputRule;
        private SampleDropdownInputLinterRule _DropdownRule;
        private NoGroupsLinterRule _noGroupsRule;

        private LinterSettings _linterSettings;

        public override void Ready(ReadyParams rp)
        {
            //load our settings
            var extensionDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)?.Replace("bin","extra");
            var settingsFile = Path.Combine(extensionDirectory, "LinterSettings.xml");
            //if the settings file exists, use it, if not load with default 5 ungrouped allowed
            _linterSettings = File.Exists(settingsFile) ? LinterSettings.DeserializeModels(settingsFile) : new LinterSettings(){AllowedUngroupedNodes = 5};


            //since we are inheriting from the LinterExtensionBase we need to mark it as ready here
            base.Ready(rp);

            //add each of our rules to the linter
            _sliderInputRule = new SampleSliderInputLinterRule();
            AddLinterRule(_sliderInputRule);

            _DropdownRule = new SampleDropdownInputLinterRule();
            AddLinterRule(_DropdownRule);

            _noGroupsRule = new NoGroupsLinterRule(_linterSettings);
            AddLinterRule(_noGroupsRule);
        }

      


        public override void Shutdown()
        {
            RemoveLinterRule(_sliderInputRule);
            RemoveLinterRule(_DropdownRule);
            RemoveLinterRule(_noGroupsRule);
        }

       
    }
}
