using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGen.AppGenLib
{
    public class FeatureWithParameters
    {
        public string featureName { get; }
        public string abstractFeatureName { get; }
        public string longClickTO { get; }
        public string longClickFROM { get; }
        public string clickTO { get; }
        public string clickFROM { get; }
        public bool required { get; }


        public string xorGroup { get; }
        public string orGroup { get; }
        public string andGroup { get; }

        public string xorAbstractGroup { get; }
        public string orAbstractGroup { get; }
        public string andAbstractGroup { get; }

        public string swipeRightTO { get; }
        public string onButtonClickFromArgTO { get; }
        public FeatureWithParameters(string featureName = null,
                                     string abstractFeatureName = null,
                                     bool required = false,
                                     string longClickTO = null,
                                     string longClickFROM = null,
                                     string clickTO = null,
                                     string clickFROM = null,
                                     string xorGroup = null,
                                     string orGroup = null,
                                     string andGroup = null,
                                     string xorAbstractGroup = null,
                                     string orAbstractGroup = null,
                                     string andAbstractGroup = null,
                                     string swipeRightTO = null,
                                     string onButtonClickFromArgTO = null)
        {
            this.featureName = featureName;
            this.abstractFeatureName = abstractFeatureName;
            this.required = false;
            this.longClickTO = longClickTO;
            this.longClickFROM = longClickFROM;
            this.clickTO = clickTO;
            this.clickFROM = clickFROM;
            this.xorGroup = xorGroup;
            this.orGroup = orGroup;
            this.andGroup = andGroup;
            this.xorAbstractGroup = xorAbstractGroup;
            this.orAbstractGroup = orAbstractGroup;
            this.andAbstractGroup = andAbstractGroup;
            this.swipeRightTO = swipeRightTO;
            this.onButtonClickFromArgTO = onButtonClickFromArgTO;
        }
    }
}