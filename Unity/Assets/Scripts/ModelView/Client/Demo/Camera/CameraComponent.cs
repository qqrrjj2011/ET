using UnityEngine;

namespace ET.Client
{
    [ComponentOf]
    public class CameraComponent:Entity,IAwake<Transform,Transform>,IUpdate,ILateUpdate
    {
        public Transform target;
        public Transform cameraTr;
        //相机旋转角度
        public Vector3 CameraRotation;
        // Exposed vars for the camera position from the target.
        //从目标到摄像机位置的外露vars.
        public float height = 20f;
        public float distance = 20f;
 
        // Options.
        //public bool doRotate;
        //相机旋转以及缩放功能开关
        public bool doZoom = true;
        public bool doRotate = true;

        // The movement amount when zooming.缩放时的移动量。
        public float zoomStep = 30f;
        public float zoomSpeed = 5f;
        public float heightWanted;
        public float distanceWanted;


        public float xSpeed = 0.8f;
        public float ySpeed = 0.8f;

        public float yMinLimit = 10f;
        public float yMaxLimit = 80f;

        //public float xMinLimit = 30f;
        //public float xMaxLimit = 220f;

        public float distanceMin = 5f;
        public float distanceMax = 60f;
        
        public float heightMin = 5f;
        public float heightMax = 60f;

        public float smoothTime = 2f;

        public float rotationYAxis = 230.0f;
        public float rotationXAxis = -8.0f;

        public float velocityX = 0.0f;
        public float velocityY = 0.0f;

        //两根手指
        public Touch oldTouch1;
        public Touch oldTouch2;
        //Vector2 m_screenPos = Vector2.zero; //记录手指触碰的位置

        public float scaleFactor;

        public float mousePosX = 0.0f;
        public float mousePosY = 0.0f;

        // Result vectors.
        public Vector3 zoomResult;//缩放后坐标
        public Quaternion rotationResult;
        public Vector3 targetAdjustedPosition;
        public Quaternion rotation;

        public bool pause { get; set; } = false;
        
        public float InitAngle = -90;
        public float CurrAngle = 20;
        public float WantedScale = 20;//想要的缩进大小
        
        
        public Vector3 Position;//当前摄像机的位置
        public Vector3 Rotation;//当前摄像机的角度

        public bool IsInit = false;
    }
    
}

