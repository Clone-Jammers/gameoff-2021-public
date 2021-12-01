using Components;
using Components.ChaosMode;
using Components.Levels;
using Components.DevMode;
using Components.Player;

namespace Managers
{
    public static class SceneManager
    {
        public static ChaosModeManager ChaosModeManager;
        public static CrosshairJuker Juker;
        public static LowFriction LowFriction;
        public static TeleportToAmmo TeleportToAmmo;
        public static TurnYouUpsideDown TurnYouUpsideDown;
    
        public static CameraFeedback CameraFeedback;
        public static PlayerController PlayerController;
        public static PlayerHudController PlayerHudController;
        public static PlayerWeaponController PlayerWeaponController;
        public static PlayerCharacterController PlayerCharacterController;
        public static PlayerCollectibleDetector PlayerCollectibleDetector;

        public static DeathMenu DeathMenu;
        public static PauseMenu PauseMenu;

        public static FirstScene FirstScene;
        public static DevModeController DevModeController;
    }
}