#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace Uft.UnityUtils
{
    /// <summary>2D IKチェーンを各ボーンのlocalPositionを操作して伸縮させて、targetまで届く見た目を作る補助。</summary>
    class StretchIkChain2D : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform? _rootBone;
        [SerializeField] Transform? _tipBone;
        [SerializeField] Transform? _target;

        [Header("Stretch Settings")]
        [SerializeField] bool _allowsSquash = false;
        [SerializeField, Min(0.01f)] float _minFactor = 0.8f;
        [SerializeField, Min(0.01f)] float _maxFactor = 1.25f;
        [SerializeField, Range(0f, 0.5f)] float _stretchBuffer = 0f;

        Transform[]? _chain;
        Vector3[]? _initialLocalPositions;
        float _initialChainLength;

        void OnEnable()
        {
            this.CaptureInitialState();
            if (Application.isPlaying) this.ApplyStretch();
        }

        void OnDisable()
        {
            this.RestoreInitialPositions();
        }

        void OnValidate()
        {
            if (this._rootBone == null) return;
            this._maxFactor = Mathf.Max(this._maxFactor, this._minFactor);
            this.RestoreInitialPositions();
            this.CaptureInitialState();
        }

        void Update()
        {
            this.ApplyStretch();
        }

        void CaptureInitialState()
        {
            this._chain = null;
            this._initialLocalPositions = null;

            if (this._rootBone == null || this._tipBone == null) return;

            var bones = new List<Transform>();
            var current = this._tipBone;
            while (current != null && current != this._rootBone)
            {
                bones.Add(current);
                current = current.parent;
            }
            if (current != this._rootBone) return;

            bones.Reverse();
            this._chain = bones.ToArray();
            this._initialLocalPositions = new Vector3[this._chain.Length];

            var chainLength = 0f;
            for (var i = 0; i < this._chain.Length; i++)
            {
                this._initialLocalPositions[i] = this._chain[i].localPosition;
                var parentPos = i == 0 ? this._rootBone.position : this._chain[i - 1].position;
                chainLength += Vector3.Distance(parentPos, this._chain[i].position);
            }
            this._initialChainLength = chainLength;
        }

        void RestoreInitialPositions()
        {
            if (this._chain == null || this._initialLocalPositions == null) return;
            for (var i = 0; i < this._chain.Length; i++)
                if (this._chain[i] != null)
                    this._chain[i].localPosition = this._initialLocalPositions[i];
        }

        void ApplyStretch()
        {
            if (this._rootBone == null || this._tipBone == null || this._target == null) return;
            if (this._chain == null || this._initialLocalPositions == null) return;

            this.RestoreInitialPositions();
            if (this._initialChainLength <= 0.0001f) return;

            var targetDistance = Vector3.Distance(this._rootBone.position, this._target.position);
            var factor = targetDistance / this._initialChainLength;

            if (factor > 1f) factor *= 1f + this._stretchBuffer;
            if (!this._allowsSquash && factor < 1f) factor = 1f;
            factor = Mathf.Clamp(factor, this._minFactor, this._maxFactor);

            for (var i = 0; i < this._chain.Length; i++)
                if (this._chain[i] != null)
                    this._chain[i].localPosition = this._initialLocalPositions[i] * factor;
        }

        void OnDrawGizmosSelected()
        {
            if (this._rootBone == null) return;
            if (this._initialChainLength > 0.0001f)
                Gizmos.DrawWireSphere(this._rootBone.position, this._initialChainLength);
            else if (this._tipBone != null)
                Gizmos.DrawWireSphere(this._rootBone.position, Vector3.Distance(this._rootBone.position, this._tipBone.position));
        }
    }
}
