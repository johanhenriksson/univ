using System;
using OpenTK;
using OpenTK.Input;

namespace univ
{
    public class Camera : Component
    {
        public Vector3 Forward { get { return this.forward; } }

        public Vector3 Right { get { return this.right; } }

        public Vector3 LookAt { get { return this.lookAt; } }

        public float AspectRatio { get { return this.aspectRatio; } }
        
        /* ViewProjection is public to allow by-reference calls */
        public Matrix4 ViewProjection;
        protected float pitch = 0.0f;
        protected float yaw = (float)Math.PI / 2.0f;
        protected Vector3 forward;
        protected Vector3 right;
        protected Vector3 lookAt;
        protected int width;
        protected int height;
        protected float aspectRatio;
     
        /* Transformation Matricies */
        protected Matrix4 view;
        protected Matrix4 projection;
 
        /* Input State */
        protected KeyboardState keystate;
        protected MouseState currentMouse, previousMouse;
        protected float speed = 4.0f;
        protected float keyTurnSpeed = 1.8f;
        protected float mouseSpeed = 0.005f;
     
        public Camera(int screenWidth, int screenHeight)
            : base()
        {
            this.height = screenHeight;
            this.width = screenWidth;
            this.aspectRatio = (float)screenWidth / screenHeight;
            this.position = new Vector3(0, 0, -2);
   
            /* Calculate vertical field of view based on screen resolution */
            float fovy = 2.0f * (float)Math.Atan(Math.Tan(Math.PI / 4.0) * (double)height / width);
            
            this.projection = Matrix4.CreatePerspectiveFieldOfView(fovy, this.aspectRatio, 0.1f, 100.0f);
            this.ViewProjection = Matrix4.Identity;
         
            /* Set up an initial previous mouse state to avoid strange behaviour
          * on the first frame */
            this.previousMouse = Mouse.GetState();
        }
     
        public override void Update(float dt)
        {
            handleKeyboard(dt);
            handleMouse(dt);
            
            /* Pitch limit */
            float PitchLimit = (float)Math.PI * 0.49f;
            if (pitch > PitchLimit)
                pitch = PitchLimit;
            if (pitch < -PitchLimit)
                pitch = -PitchLimit;
         
            /* Optimization */
            float sin_p = (float)Math.Sin(pitch);
            float cos_p = (float)Math.Cos(pitch);
            float sin_y = (float)Math.Sin(yaw);
            float cos_y = (float)Math.Cos(yaw);
         
            /* Calculate updated direction vectors */
            forward.X = cos_y;               /* sin(yaw) */
            forward.Z = sin_y;               /* cos(yaw) */ 
            right.X = -sin_y;                /* -cos(yaw) */
            right.Z = cos_y;                 /* sin(yaw) */ 
            lookAt.X = cos_y * cos_p;        /* cos(yaw) * cos(pitch) */
            lookAt.Y = sin_p;                /* sin(pitch) */
            lookAt.Z = sin_y * cos_p;        /* sin(yaw) * cos(pitch) */
         
            /* Recalculate View Projection matrix */
            Vector3 target = position + lookAt;
            this.view = Matrix4.LookAt(this.position, target, Vector3.UnitY);
            Matrix4.Mult(ref view, ref projection, out ViewProjection);      
        }
        
        public Vector4 UnProject(int x, int y)
        {
            Vector2 mouse = new Vector2(x, y);
            Vector4 vec;
 
            vec.X = 2.0f * mouse.X / (float)this.width - 1;
            vec.Y = -(2.0f * mouse.Y / (float)this.height - 1);
            vec.Z = 0;
            vec.W = 1.0f;
 
            Matrix4 viewInv = Matrix4.Invert(view);
            Matrix4 projInv = Matrix4.Invert(projection);
 
            Vector4.Transform(ref vec, ref projInv, out vec);
            Vector4.Transform(ref vec, ref viewInv, out vec);
 
            if (vec.W > float.Epsilon || vec.W < float.Epsilon) {
                vec.X /= vec.W;
                vec.Y /= vec.W;
                vec.Z /= vec.W;
            }
 
            return vec;
        }
        
        protected void handleKeyboard(float dt)
        {
            keystate = Keyboard.GetState();

            /* Lateral movement */
            if (keystate.IsKeyDown(Key.W))
                position += forward * speed * dt;
            if (keystate.IsKeyDown(Key.S))
                position -= forward * speed * dt;
            if (keystate.IsKeyDown(Key.A))
                position -= right * speed * dt;
            if (keystate.IsKeyDown(Key.D))
                position += right * speed * dt;
                 
            /* Fly up/down */
            if (keystate.IsKeyDown(Key.Space))
                position.Y += speed * dt;
            if (keystate.IsKeyDown(Key.LShift))
                position.Y -= speed * dt;
                 
            /* Keyboard turn */
            if (keystate.IsKeyDown(Key.Left))
                yaw -= keyTurnSpeed * dt;
            if (keystate.IsKeyDown(Key.Right))
                yaw += keyTurnSpeed * dt;
            if (keystate.IsKeyDown(Key.Up))
                pitch += keyTurnSpeed * dt;
            if (keystate.IsKeyDown(Key.Down))
                pitch -= keyTurnSpeed * dt;
        }
        
        protected void handleMouse(float dt)
        {
            currentMouse = Mouse.GetState(); 
            
            if (currentMouse.RightButton == ButtonState.Pressed) {
                float dx = currentMouse.X - previousMouse.X;
                float dy = currentMouse.Y - previousMouse.Y;
            
                yaw += mouseSpeed * dx;
                pitch -= mouseSpeed * dy;
            }
            
            this.previousMouse = currentMouse;
        }
    }
}

