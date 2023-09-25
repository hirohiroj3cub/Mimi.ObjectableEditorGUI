using System;
using UnityEditor;

namespace Mimi.InspectorAnimation
{
    public class InspectorAnimator
    {
        private class Updater
        {
            public Updater()
            {
                UpdateGroups = new InspectorUpdateGroup.List();
            }

            public InspectorUpdateGroup.List UpdateGroups { get; }

            public void RefreshUpdateGroup(InspectorAnimator animator)
            {
                var editors = InspectorWindows.GetEditors(animator.KeyObject);
                var newUpdateGroups = manager.GetUpdateGroups(editors, animator.FrameRate);
                UpdateGroups.Refresh(animator, newUpdateGroups!);
            }

            public void ForceRepaint(InspectorAnimator animator)
            {
                RefreshUpdateGroup(animator);
                UpdateGroups.ForceRepaint();
                manager.TryStartUpdate();
            }

            public void StartAnimation(InspectorAnimator animator)
            {
                RefreshUpdateGroup(animator);
                manager.TryStartUpdate();
            }
        }

        private class BackgroundWorker
        {
            private static bool runBackgroundWorker = false;

            private static void StartBackgroundWorker()
            {
                EditorApplication.update += BackgroundWorker;

                static void BackgroundWorker()
                {
                    if (UnityEditorInternal.InternalEditorUtility.isApplicationActive)
                    {
                        EditorApplication.update -= BackgroundWorker;
                        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                    }
                }
            }

            private bool isActive = !UnityEditorInternal.InternalEditorUtility.isApplicationActive;
            private bool backgroundRestartPlay = false;
            private bool backgroundRestartRepeat = false;

            public void Check(InspectorAnimator animator)
            {
                var newIsActive = UnityEditorInternal.InternalEditorUtility.isApplicationActive;
                if (isActive != newIsActive)
                {
                    isActive = newIsActive;

                    if (isActive)
                    {
                        OnBackground(animator);
                    }
                    else
                    {
                        OnActive(animator);
                    }
                }
            }

            private void OnActive(InspectorAnimator animator)
            {
                backgroundRestartPlay = animator.IsPlaying;
                backgroundRestartRepeat = animator.IsRepeating;
                if (backgroundRestartPlay || backgroundRestartRepeat)
                {
                    animator.Stop();
                    if (!runBackgroundWorker)
                    {
                        runBackgroundWorker = true;
                        StartBackgroundWorker();
                    }
                }
            }

            private void OnBackground(InspectorAnimator animator)
            {
                if (backgroundRestartPlay || backgroundRestartRepeat || runBackgroundWorker)
                {
                    if (backgroundRestartRepeat)
                    {
                        animator.ContinueRepeat();
                    }
                    else if (backgroundRestartPlay)
                    {
                        animator.Continue();
                    }

                    backgroundRestartPlay = false;
                    backgroundRestartRepeat = false;
                    runBackgroundWorker = false;
                }
            }
        }

        private static readonly InspectorAnimatorManager manager = new InspectorAnimatorManager();

        private static int TimeSpanDivRem(TimeSpan a, TimeSpan b, out TimeSpan floor, out TimeSpan rem)
        {
            int count;
            if (a < TimeSpan.Zero)
            {
                count = -1;
                while (a < b * count) --count;
            }
            else
            {
                count = 0;
                while (a >= b * (count + 1)) ++count;
            }

            floor = b * count;
            rem = a - floor;
            return count;
        }

        private readonly Updater updater;
        private readonly BackgroundWorker backgroundRunner;

        private bool isStatic;
        private readonly TimeSpan duration;
        private readonly double frameRate;
        private bool isRepeating;
        private readonly TimeSpan frameDelta;
        private bool isInit;
        private DateTime? startTime;
        private DateTime? previousStartTime;
        private DateTime lastUpdateTime;
        private TimeSpan? canceledAnimateTime;
        private TimeSpan previousTotalTime;
        private SerializedObject? serializedObject;

        public InspectorAnimator(double duration, bool isRepeating = false, double frameRate = 30.0)
            : this(TimeSpan.FromSeconds(duration), isRepeating, frameRate) { }

        public InspectorAnimator(TimeSpan duration, bool isRepeating = false, double frameRate = 30.0)
        {
            updater = new Updater();
            backgroundRunner = new BackgroundWorker();

            this.duration = duration;
            this.frameRate = Math.Clamp(frameRate, 0.1, 60.0);
            this.isRepeating = isRepeating;
            frameDelta = TimeSpan.FromSeconds(1.0 / frameRate);
            isStatic = false;
            isInit = true;
            startTime = null;
            previousStartTime = null;
            lastUpdateTime = DateTime.MinValue;
            canceledAnimateTime = null;
            previousTotalTime = TimeSpan.Zero;
            serializedObject = null;
        }

        public TimeSpan Duration => duration;
        public double FrameRate => frameRate;
        public bool IsRepeating => isRepeating;
        public TimeSpan FrameDelta => frameDelta;
        public bool IsStatic => isStatic;

        public DateTime? StartTime => startTime;
        public DateTime? PreviousStartTime => previousStartTime;
        public DateTime LastUpdateTime => lastUpdateTime;
        public TimeSpan? CanceledAnimateTime => canceledAnimateTime;
        public TimeSpan PreviousTotalTime => previousTotalTime;
        public SerializedObject? SerializedObject => serializedObject;

        public object? KeyObject => GetKeyObject();

        public bool IsPlaying => startTime.HasValue;
        public bool IsCanceled => !startTime.HasValue && previousStartTime.HasValue && canceledAnimateTime.HasValue;
        public bool IsFinished => !startTime.HasValue && previousStartTime.HasValue && !canceledAnimateTime.HasValue;
        public TimeSpan EndAnimateTime => canceledAnimateTime ?? Duration;

        public DateTime? CanceledTime => IsCanceled ? previousStartTime + canceledAnimateTime : null;
        public DateTime? FinishTime => startTime.HasValue ? startTime + Duration : null;
        public DateTime? FinishedTime => previousStartTime.HasValue ? previousStartTime + Duration : null;
        public DateTime? EndTime => previousStartTime.HasValue ? previousStartTime + EndAnimateTime : null;

        public TimeSpan AnimateTime
        {
            get => GetAnimateTime();
            set => SetAnimateTime(value);
        }
        public int AnimateFrame => GetFrame(AnimateTime);
        public TimeSpan AnimateFrameTime => GetFrameTime(AnimateTime, TimeSpan.Zero);

        public TimeSpan TotalAnimateTime
        {
            get => previousTotalTime + GetAnimateTime();
            set => SetTotalAnimateTime(value);
        }
        public int TotalAnimateFrame => GetFrame(TotalAnimateTime);
        public TimeSpan TotalAnimateFrameTime => GetFrameTime(TotalAnimateTime, previousTotalTime);

        public bool IsNotUpdated => IsNotUpdatedAt(DateTime.Now);
        public TimeSpan NonUpdateTime => NonUpdateTimeAt(DateTime.Now);

        public void InitNewDrawer()
        {
            isInit = true;
        }

        public void OnGUIUpdate(SerializedObject serializedObject)
        {
            var updateTime = DateTime.Now;
            bool isStopping = IsNotUpdatedAt(updateTime);
            var nonUpdateTime = NonUpdateTimeAt(updateTime);

            lastUpdateTime = updateTime;

            OnTargetSetup(serializedObject);

            if (isInit)
            {
                OnInit(nonUpdateTime);
            }
            else if (isStopping)
            {
                OnStopping(nonUpdateTime);
            }

            if (IsPlaying && lastUpdateTime > startTime!.Value + Duration)
            {
                End();
            }

            if (isRepeating)
            {
                OnRepeating();
            }

            backgroundRunner.Check(this);
        }

        public bool Start()
        {
            if (IsPlaying) return false;

            previousTotalTime = TotalAnimateTime;
            SetPlayTime(LastUpdateTime, TimeSpan.Zero);
            updater.StartAnimation(this);

            return true;
        }

        public bool Continue()
        {
            if (IsPlaying) return false;
            if (!IsCanceled) return Start();

            SetPlayTime(canceledAnimateTime!.Value);

            updater.StartAnimation(this);
            return true;
        }

        public bool Stop()
        {
            if (!IsPlaying && !IsRepeating) return false;

            isRepeating = false;
            SetStopTime(startTime!.Value, AnimateTime);

            return true;
        }

        public bool StartRepeat()
        {
            if (isRepeating) return false;

            isRepeating = true;
            return Start();
        }

        public bool ContinueRepeat()
        {
            if (isRepeating) return false;

            isRepeating = true;
            return Continue();
        }

        public bool StopRepeat()
        {
            if (!isRepeating) return false;
            isRepeating = false;
            return true;
        }

        private object? GetKeyObject()
        {
            if (SerializedObject == null) return null;
            if (isStatic) return SerializedObject.targetObject;
            return SerializedObject;
        }

        private bool IsNotUpdatedAt(DateTime dateTime)
        {
            return (IsPlaying || IsRepeating) && NonUpdateTimeAt(dateTime) > FrameDelta * 10.0;
        }

        private TimeSpan NonUpdateTimeAt(DateTime dateTime)
        {
            return dateTime - LastUpdateTime;
        }

        private TimeSpan GetAnimateTime()
        {
            if (IsCanceled) return canceledAnimateTime!.Value;
            if (IsFinished) return duration;
            if (IsPlaying) return lastUpdateTime - startTime!.Value;
            return TimeSpan.Zero;
        }

        private int GetFrame(TimeSpan time)
        {
            return TimeSpanDivRem(time, FrameDelta, out _, out _);
        }

        private TimeSpan GetFrameTime(TimeSpan time, TimeSpan min)
        {
            if (time <= min) return min;
            var max = min + Duration;
            if (time >= max) return max;
            TimeSpanDivRem(time, FrameDelta, out var result, out _);
            return result;
        }

        private void SetAnimateTime(TimeSpan animateTime)
        {
            if (IsPlaying || IsRepeating)
            {
                SetPlayTime(animateTime);

            }
            else
            {
                SetStopTime(animateTime);
            }

            updater.ForceRepaint(this);
        }

        private void SetTotalAnimateTime(TimeSpan value)
        {
            TimeSpanDivRem(value, Duration, out var prevTotalTime, out var animateTime);
            previousTotalTime = prevTotalTime;
            SetAnimateTime(animateTime);
        }

        private void SetPlayTime(TimeSpan animateTime) => SetPlayTime(lastUpdateTime - animateTime, animateTime);
        private void SetPlayTime(DateTime startTime, TimeSpan animateTime)
        {
            if (animateTime < Duration)
            {
                previousStartTime = this.startTime;
                canceledAnimateTime = null;
                this.startTime = startTime;
            }
            else
            {
                End();
            }
        }

        private void SetStopTime(TimeSpan canceledAnimateTime) => SetStopTime(lastUpdateTime - canceledAnimateTime, canceledAnimateTime);
        private void SetStopTime(DateTime startTime, TimeSpan canceledAnimateTime)
        {
            if (canceledAnimateTime <= TimeSpan.Zero)
            {
                previousStartTime = null;
                this.canceledAnimateTime = null;
                this.startTime = null;
            }
            else if (canceledAnimateTime <= Duration)
            {
                previousStartTime = startTime;
                this.canceledAnimateTime = canceledAnimateTime;
                this.startTime = null;
            }
            else
            {
                this.startTime = startTime;
                End();
            }
        }

        private void End()
        {
            previousStartTime = startTime;
            canceledAnimateTime = null;
            startTime = null;
        }

        private void OnTargetSetup(SerializedObject serializedObject)
        {
            if (serializedObject != this.serializedObject)
            {
                if (!isStatic && this.serializedObject != null)
                {
                    isStatic = true;
                }
                this.serializedObject = serializedObject;
            }
        }

        private void OnInit(TimeSpan nonUpdateTime)
        {
            isInit = false;

            if (IsPlaying || IsRepeating)
            {
                startTime = startTime! + nonUpdateTime;
                updater.StartAnimation(this);
            }
        }

        private void OnStopping(TimeSpan nonUpdateTime)
        {
            if (IsPlaying || IsRepeating)
            {
                startTime = startTime! + nonUpdateTime;
                updater.StartAnimation(this);
            }
        }

        private void OnRepeating()
        {
            if (IsFinished)
            {
                previousTotalTime += duration;
                SetPlayTime(FinishedTime!.Value, TimeSpan.Zero);
            }
            else
            {
                Continue();
            }
        }
    }
}