using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(CameraComponent))]
    [FriendOf(typeof(CameraComponent))]
    public static partial class CameraComponentSystem
    {
        [EntitySystem]
        private static void Awake(this CameraComponent self,Transform target,Transform cameraTr)
        {
            self.target = target;
            self.cameraTr = cameraTr;
            self.Position = self.cameraTr.position;
            self.rotation = self.cameraTr.rotation;

            //得到相机欧拉角
            Vector3 angles = self.cameraTr.eulerAngles;
            //相机绕Y轴转动的角度值
            self.rotationYAxis = angles.y;
            //相机绕X轴转动的角度值
            self. rotationXAxis = angles.x;
            Log.Info("相机初始位置" + self.rotationXAxis);
            //print("Y轴数值"+ rotationYAxis);
            //print("X轴数值" + rotationXAxis);

            // Initialise default zoom vals.
            //相机当前高度赋值于目标高度
            self.heightWanted = self.height;
            self.distanceWanted = self.distance;
            // Setup our default camera. We set the zoom result to be our default position.
            self.zoomResult = new Vector3(0f, self.height, -self.distance);

        }
        [EntitySystem]
        private static void Update(this CameraComponent self)
        {

        }

        [EntitySystem]
        private static void LateUpdate(this CameraComponent self)
        {
            if (self.pause) return;
            if (self.IsInit == true)
            {
                self.distanceWanted = self.WantedScale;
                //   AsianAni.Instance.Tween(InitAngle);
                self.rotationXAxis = 18;
                
               //1111 DOTween.To(() => rotationXAxis, x => rotationXAxis = x, CurrAngle, 0.8f);
                self.rotationYAxis = 0;
                self.IsInit = false;
            }

            // Check target.
            //检测目标是否存在
            if (!self.target)
            {
                Log.Error("This camera has no target, you need to assign a target in the inspector.");
                return;
            }

            //相机视角缩放
            if (self.doZoom)
            {
                //print(doRotate);
                //if (Input.touchCount <= 0)
                //{
                // return;
                //}
                float mouseInput;
                if (Input.touchCount > 1)
                {

                    Touch newTouch1 = Input.GetTouch(0);
                    Touch newTouch2 = Input.GetTouch(1);
                    //第2点刚开始接触屏幕, 只记录，不做处理  
                    if (newTouch2.phase == TouchPhase.Began)
                    {
                        self.oldTouch2 = newTouch2;
                        self.oldTouch1 = newTouch1;
                        //return;
                    }

                    //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型  
                    float oldDistance = Vector2.Distance(self.oldTouch1.position, self.oldTouch2.position);
                    float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);
                    //两个距离只差，为正表示放大，为负表示缩小
                    float offset = newDistance - oldDistance;
                    //缩放因子
                    self.scaleFactor = offset / 1000f;

                    mouseInput = self.scaleFactor;

                    self.heightWanted -= self.zoomStep * mouseInput;
                    self.distanceWanted -= self.zoomStep * mouseInput;
                }

                // Record our mouse input. If we zoom add this to our height and distance.
                //记录鼠标滚轮滚动时的变量 并赋值记录
                //mouseInput特性：正常状态为0；滚轮前推一格变为+0.1一次，后拉则变为-0.1一次
                // Input.GetAxis("Mouse ScrollWheel");
                if (Input.touchCount <= 0)
                {
                    mouseInput = Input.GetAxis("Mouse ScrollWheel");

                    self.heightWanted -= self.zoomStep * mouseInput;
                    self.distanceWanted -= self.zoomStep * mouseInput;
                }
                //print("+++"+mouseInput);

                // Make sure they meet our min/max values.
                //限制相机高度范围
                self.heightWanted = Mathf.Clamp(self.heightWanted, self.heightMin, self.heightMax);
                self.distanceWanted = Mathf.Clamp(self.distanceWanted, self.distanceMin, self.distanceMax);
                //差值计算，动态修改相机高度值（平滑的变化）
                self.height = Mathf.Lerp(self.height, self.heightWanted, Time.deltaTime * self.zoomSpeed);
                self.distance = Mathf.Lerp(self.distance, self.distanceWanted, Time.deltaTime * self.zoomSpeed);

                // Post our result.
                //缩放后坐标
                self.zoomResult = new Vector3(0f, self.height, -self.distance);
            }

            //相机视角旋转
            if (self.doRotate)
            {
                //print("水平" + Input.GetAxis("Horizontal"));
                //print("竖直" + Input.GetAxis("Vertical"));
                if (Input.touchCount == 1)
                {
                    Touch newTouch1 = Input.GetTouch(0);
                    //Touch touch = Input.GetTouch(0);
                    if (Input.touches[0].phase == TouchPhase.Began)
                    {
                        self.oldTouch1 = newTouch1;
                        //m_screenPos = touch.position;
                    }

                    if (Input.touches[0].phase == TouchPhase.Moved)
                    {
                        float CX = newTouch1.position.x - self.oldTouch1.position.x;
                        float CY = newTouch1.position.y - self.oldTouch1.position.y;

                        self.velocityX += self.xSpeed * CX * 0.002f * Time.deltaTime;
                        self.velocityY += self.ySpeed * CY * 0.002f * Time.deltaTime;
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    self.mousePosX = Input.mousePosition.x;
                    self.mousePosY = Input.mousePosition.y;
                }

                if (Input.GetMouseButton(0))
                {
                    // print("欧拉角"+transform.eulerAngles);
                    self.velocityX += self.xSpeed * (Input.mousePosition.x - self.mousePosX) * 0.02f;
                    self.velocityY += self.ySpeed * (Input.mousePosition.y - self.mousePosY) * 0.02f;

                    self.mousePosX = Input.mousePosition.x;
                    self.mousePosY = Input.mousePosition.y;
                }

                self.rotationYAxis += self.velocityX;
                self.rotationXAxis -= self.velocityY;
                if (self.rotationXAxis >= self.yMaxLimit)
                {
                    self.rotationXAxis = self.yMaxLimit;
                }
                else if (self.rotationXAxis <= self.yMinLimit)
                {
                    self.rotationXAxis = self.yMinLimit;
                }

            }

            Quaternion toRotation = Quaternion.Euler(self.rotationXAxis, self.rotationYAxis, 0);
            Quaternion rotation = toRotation;
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -self.distance);
            //相机跟随
            Vector3 position = rotation * negDistance + self.target.position;
            //改变相机Rotation，从而旋转相机
            self.cameraTr.rotation = rotation;

            
            //将缩放后的坐标作为相机的当前坐标位置
            self.cameraTr.position = position;
            self.velocityX = Mathf.Lerp(self.velocityX, 0, Time.deltaTime * self.smoothTime);
            self.velocityY = Mathf.Lerp(self.velocityY, 0, Time.deltaTime * self.smoothTime);
        }

        public static float ClampAngle(this CameraComponent self, float angle, float min, float max)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;
            //限制相机转动角度
            return Mathf.Clamp(angle, min, max);
        }

        public static void InitPoint(this CameraComponent self)
        {
            self.heightWanted = self.heightMax;
            self.distanceWanted = self.distanceMax;
        }

        public static void InitReturn(this CameraComponent self,float a, float b)
        {
            self.heightWanted = a;
            self.distanceWanted = b;
        }
    }
}

