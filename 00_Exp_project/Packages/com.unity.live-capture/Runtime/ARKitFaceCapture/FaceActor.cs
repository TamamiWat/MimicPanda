using UnityEngine;

namespace Unity.LiveCapture.ARKitFaceCapture
{
    /// <summary>
    /// A component used to apply face poses to a character in a scene.
    /// </summary>
    [DisallowMultipleComponent]
    [ExcludeFromPreset]
    [AddComponentMenu("Live Capture/ARKit Face Capture/ARKit Face Actor")]
    [ExecuteAlways]
    [RequireComponent(typeof(Animator))]
    [HelpURL(Documentation.baseURL + Documentation.version + Documentation.subURL + "ref-component-arkit-face-actor" + Documentation.endURL)]
    public sealed class FaceActor : MonoBehaviour
    {
        internal static class PropertyNames
        {
            public const string BlendShapes = nameof(m_BlendShapes);
            public const string HeadPosition = nameof(m_HeadPosition);
            public const string HeadOrientation = nameof(m_HeadOrientation);
            public const string LeftEyeOrientation = nameof(m_LeftEyeOrientation);
            public const string RightEyeOrientation = nameof(m_RightEyeOrientation);
            public const string BlendShapesEnabled = nameof(m_BlendShapesEnabled);
            public const string HeadPositionEnabled = nameof(m_HeadPositionEnabled);
            public const string HeadOrientationEnabled = nameof(m_HeadOrientationEnabled);
            public const string EyeOrientationEnabled = nameof(m_EyeOrientationEnabled);
            public const string MimicModeEnabled = nameof(m_MimicModeEnabled);
        }

        [SerializeField, Tooltip("The asset that configures how face pose data is mapped to this character's face rig.")]
        FaceMapper m_Mapper = null;

        [SerializeField, Tooltip("The channels of face capture data to apply to this actor. " +
            "This allows for recording all channels of a face capture, while later being able to use select parts of the capture.")]
        [EnumFlagButtonGroup(100f)]
        FaceChannelFlags m_EnabledChannels = FaceChannelFlags.All;

        [SerializeField, Tooltip("The blend shapes weights that define the face expression.")]
        FaceBlendShapePose m_BlendShapes;
        [SerializeField, Tooltip("The position of the head.")]
        Vector3 m_HeadPosition;
        [SerializeField, Tooltip("The rotation of the head.")]
        Vector3 m_HeadOrientation = Vector3.zero;
        [SerializeField, Tooltip("The rotation of the left eye.")]
        Vector3 m_LeftEyeOrientation = Vector3.zero;
        [SerializeField, Tooltip("The rotation of the right eye.")]
        Vector3 m_RightEyeOrientation = Vector3.zero;
        [SerializeField]
        bool m_BlendShapesEnabled;
        [SerializeField]
        bool m_HeadPositionEnabled;
        [SerializeField]
        bool m_HeadOrientationEnabled;
        [SerializeField]
        bool m_EyeOrientationEnabled;
        [SerializeField]
        bool m_MimicModeEnabled;
        //bool isMix;
        //public bool mimicMODE = false;

        FaceMapperCache m_Cache;
        FaceChannelFlags m_LastChannels;

        /// <summary>
        /// The Animator component used by the device for playing takes on this actor.
        /// </summary>
        public Animator Animator { get; private set; }

        /// <summary>
        /// The asset that configures how face pose data is mapped to this character's face rig.
        /// </summary>
        public FaceMapper Mapper => m_Mapper;

        internal FaceBlendShapePose BlendShapes
        {
            get => m_BlendShapes;
            set => m_BlendShapes = value;
        }

        internal Vector3 HeadPosition
        {
            get => m_HeadPosition;
            set => m_HeadPosition = value;
        }

        internal Vector3 HeadOrientation
        {
            get => m_HeadOrientation;
            set => m_HeadOrientation = value;
        }

        internal Vector3 LeftEyeOrientation
        {
            get => m_LeftEyeOrientation;
            set => m_LeftEyeOrientation = value;
        }

        internal Vector3 RightEyeOrientation
        {
            get => m_RightEyeOrientation;
            set => m_RightEyeOrientation = value;
        }

        internal bool BlendShapesEnabled
        {
            get => m_BlendShapesEnabled;
            set => m_BlendShapesEnabled = value;
        }

        internal bool HeadPositionEnabled
        {
            get => m_HeadPositionEnabled;
            set => m_HeadPositionEnabled = value;
        }

        internal bool HeadOrientationEnabled
        {
            get => m_HeadOrientationEnabled;
            set => m_HeadOrientationEnabled = value;
        }

        internal bool EyeOrientationEnabled
        {
            get => m_EyeOrientationEnabled;
            set => m_EyeOrientationEnabled = value;
        }

        /***********************************/
        internal bool MimicModeEnabled
        {
            get => m_MimicModeEnabled;
            set => m_MimicModeEnabled = value;
        }
        /***********************************/

        void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        void OnDisable()
        {
            ClearCache();
        }

        void LateUpdate()
        {
            UpdateRig(true);
        }

        /// <summary>
        /// Sets the face mapper used by this face rig.
        /// </summary>
        /// <param name="mapper">The mapper to use, or null to clear the current mapper.</param>
        public void SetMapper(FaceMapper mapper)
        {
            m_Mapper = mapper;
            ClearCache();
        }

        /// <summary>
        /// Clears the mapper state cache for the actor, causing it to rebuild the next
        /// time the face rig updates.
        /// </summary>
        public void ClearCache()
        {
            m_Cache?.Dispose();
            m_Cache = null;
        }

        /// <summary>
        /// Updates the face rig from the current pose.
        /// </summary>
        /// <param name="continuous">When true, the new pose follows the current pose and they
        /// can be smoothed between, while false corresponds to a seek in the animation where the
        /// previous pose is invalidated and should not influence the new pose.</param>
        public void UpdateRig(bool continuous)
        {
            if (m_Mapper != null)
            {
                if (m_Cache == null)
                {
                    m_Cache = m_Mapper.CreateCache(this);

                    // we can't use any previous state since there is none yet
                    continuous = false;
                }

                // determine which face channels are enabled and have data to use
                var channels = m_EnabledChannels;

                if (!m_BlendShapesEnabled)
                {
                    channels &= ~FaceChannelFlags.BlendShapes;
                }
                if (!m_HeadPositionEnabled)
                {
                    channels &= ~FaceChannelFlags.HeadPosition;
                }
                if (!m_HeadOrientationEnabled)
                {
                    channels &= ~FaceChannelFlags.HeadRotation;
                }
                if (!m_EyeOrientationEnabled)
                {
                    channels &= ~FaceChannelFlags.Eyes;
                }

                /**********************************/
                if (!m_MimicModeEnabled)
                {
                    channels &= ~FaceChannelFlags.MimicMode;
                }
                /**********************************/

                // apply the active face channels
                if (channels.HasFlag(FaceChannelFlags.BlendShapes))
                {
                    m_Mapper.ApplyBlendShapesToRig(
                        this,
                        m_Cache,
                        ref m_BlendShapes,
                        continuous && m_LastChannels.HasFlag(FaceChannelFlags.BlendShapes)
                    );
                }

                /*if (channels.HasFlag(FaceChannelFlags.BlendShapes))
                {
                    m_Mapper.ApplyBlendShapesToRig(
                        this,
                        m_Cache,
                        ref m_BlendShapes,
                        continuous && m_LastChannels.HasFlag(FaceChannelFlags.BlendShapes) && m_LastChannels.HasFlag(FaceChannelFlags.MimicMode)
                    );
                }*/



                /*if (!isMix && channels.HasFlag(FaceChannelFlags.BlendShapes))   //only mimic
                {
                    m_Mapper.ApplyBlendShapesToRig(
                        this,
                        m_Cache,
                        ref m_BlendShapes,
                        continuous && m_LastChannels.HasFlag(FaceChannelFlags.BlendShapes)
                    );
                }else if(isMix && channels.HasFlag(FaceChannelFlags.BlendShapes))   //mix
                {
                    if (mimicMODE)  //mimic mode
                    {
                        m_Mapper.ApplyBlendShapesToRig(
                            this,
                            m_Cache,
                            ref m_BlendShapes,
                            continuous && m_LastChannels.HasFlag(FaceChannelFlags.BlendShapes)
                        );
                    }
                    else if (!mimicMODE)    //not mimic mode
                    {

                    }
                }*/
                if (channels.HasFlag(FaceChannelFlags.HeadPosition))
                {
                    m_Mapper.ApplyHeadPositionToRig(
                        this,
                        m_Cache,
                        ref m_HeadPosition,
                        continuous && m_LastChannels.HasFlag(FaceChannelFlags.HeadPosition)
                    );
                }
                if (channels.HasFlag(FaceChannelFlags.HeadRotation))
                {
                    var headRotation = Quaternion.Euler(m_HeadOrientation[0], -m_HeadOrientation[1], -m_HeadOrientation[2]);

                    m_Mapper.ApplyHeadRotationToRig(
                        this,
                        m_Cache,
                        ref headRotation,
                        continuous && m_LastChannels.HasFlag(FaceChannelFlags.HeadRotation)
                    );
                }
                if (channels.HasFlag(FaceChannelFlags.Eyes))
                {
                    var leftEyeRotation = Quaternion.Euler(m_LeftEyeOrientation[0], -m_LeftEyeOrientation[1], -m_LeftEyeOrientation[2]);
                    var rightEyeRotation = Quaternion.Euler(m_RightEyeOrientation[0], -m_RightEyeOrientation[1], -m_RightEyeOrientation[2]);

                    m_Mapper.ApplyEyeRotationToRig(
                        this,
                        m_Cache,
                        ref m_BlendShapes,
                        ref leftEyeRotation,
                        ref rightEyeRotation,
                        continuous && m_LastChannels.HasFlag(FaceChannelFlags.Eyes)
                    );
                }

                m_LastChannels = channels;
            }
        }
    }
}
