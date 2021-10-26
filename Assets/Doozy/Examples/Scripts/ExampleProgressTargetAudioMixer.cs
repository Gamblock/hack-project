using Doozy.Engine.Progress;
using UnityEngine.Audio;

namespace Doozy.Examples
{
    public class ExampleProgressTargetAudioMixer : ProgressTarget
    {
        public AudioMixer AudioMixer;
        public string ExposedParameter;
        public TargetVariable TargetVariable = TargetVariable.Value;

        /// <summary> Internal variable used to get the updated target value </summary>
        private float m_targetValue;

        /// <inheritdoc />
        /// <summary> Method used by a Progressor to when the current Value has changed </summary>
        /// <param name="progressor"> The Progressor that triggered this update </param>
        public override void UpdateTarget(Progressor progressor)
        {
            base.UpdateTarget(progressor);

            if (AudioMixer == null) return;

            m_targetValue = 0;
            switch (TargetVariable)
            {
                case TargetVariable.Value:
                    m_targetValue = progressor.Value;
                    break;
                case TargetVariable.MinValue:
                    m_targetValue = progressor.MinValue;
                    break;
                case TargetVariable.MaxValue:
                    m_targetValue = progressor.MaxValue;
                    break;
                case TargetVariable.Progress:
                    m_targetValue = progressor.Progress;
                    break;
                case TargetVariable.InverseProgress:
                    m_targetValue = progressor.InverseProgress;
                    break;
            }

            AudioMixer.SetFloat(ExposedParameter, m_targetValue);
        }
    }
}