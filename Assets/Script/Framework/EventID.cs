
public enum EventID
{
    None = 0,
    Launch,
    NonLaunch,
    Drag,
    HoopPassed,
    BallInHoop,
    MoveCamera,
    ReachPerfect,
    ToggleNightMode,
    //State
    GameOver,
    PlayGame,
    GoHome,
    // For UI
    AddScore,
    //For Shop
    SkinClick,
    ThemeClick,
    //Challenge
    GoToChallenge,
    PlayChallenge,
    ReachFinishHoop,
    CompleteChallenge,
    UncompleteChallenge,
    BackFromChallenge,
    PassHoopChallenge,
    StartChallenge,
    TimeOut,
    RestartLevel,
    
    //For achievement
    Bounce, 
    BestScoreChange,
    GetStar,
    WatchVideo, 
    GetToken,
}
