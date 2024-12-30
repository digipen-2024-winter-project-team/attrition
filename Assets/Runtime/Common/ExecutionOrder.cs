namespace Attrition.Common
{
    /// <summary>
    /// Defines the execution order for various components in the game.
    /// </summary>
    public static class ExecutionOrder
    {
        /// <summary>
        /// Execution order for global application setup, such as bootstrapping systems that persist across scenes.
        /// </summary>
        public const int ApplicationSetup = -1500;

        /// <summary>
        /// Execution order for global game setup, such as singletons and input managers.
        /// </summary>
        public const int GameSetup = -1000;

        /// <summary>
        /// Execution order for scene-specific setup, such as environment controllers or scene pooling.
        /// </summary>
        public const int SceneSetup = -750;

        /// <summary>
        /// Execution order for initializing entities, such as NPCs or player characters.
        /// </summary>
        public const int EntitySetup = -500;

        /// <summary>
        /// Execution order for player-specific setup, such as player controllers or camera systems.
        /// </summary>
        public const int PlayerSetup = -250;

        /// <summary>
        /// Execution order for physics-related initialization, such as configuring rigidbodies and colliders.
        /// </summary>
        public const int PhysicsSetup = -100;

        /// <summary>
        /// Default execution order for most gameplay-related logic.
        /// </summary>
        public const int DefaultSetup = 0;

        /// <summary>
        /// Execution order for late frame adjustments, such as camera smoothing or final animations.
        /// </summary>
        public const int LateGameplaySetup = 750;

        /// <summary>
        /// Execution order for debugging, analytics, and logging tools, executed last.
        /// </summary>
        public const int Debugging = 1000;
    }
}
