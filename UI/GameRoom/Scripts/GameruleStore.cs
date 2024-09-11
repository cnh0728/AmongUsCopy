using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum EKillRange
{
    Short, Normal, Long
}

public enum ETaskBarUdpate
{
    Always, Meetings, Never
}

public struct GameRuleData
{
    public bool confirmEjects;
    public int emergencyMeetings;
    public int emergencyMeetingsCooldown;
    public int meetingsTime;
    public int voteTime;
    public bool anonymousVotes;
    public float moveSpeed;
    public float crewSight;
    public float imposterSight;
    public float killCooldown;
    public EKillRange killRange;
    public bool visualTasks;
    public ETaskBarUdpate taskBarUpdates;
    public int commonTask;
    public int complexTask;
    public int simpleTask;
}

public class GameruleStore : NetworkBehaviour
{
    [SyncVar(hook = nameof(SetIsRecommendRule_Hook))]
    private bool isRecommendRule;
    [SerializeField]
    private Toggle isRecommendRuleToggle;
    public void SetIsRecommendRule_Hook(bool oldValue, bool newValue)
    {
        UpdateGameRuleOverview();
    }
    public void OnRecommendToggle(bool value)
    {
        isRecommendRule = value;
        if (isRecommendRule)
        {
            SetRecommendGameRule();
        }
    }

    private void IsNotRecommendRule()
    {
        isRecommendRule = false;
        isRecommendRuleToggle.isOn = false;
    }

    [SyncVar(hook = nameof(SetConfirmEjects_Hook))]
    private bool confirmEjects;
    [SerializeField]
    private Toggle confirmEjectsToggle;
    public void SetConfirmEjects_Hook(bool oldValue, bool newValue)
    {
        UpdateGameRuleOverview();
    }
    public void OnConfirmEjectsToggle(bool value)
    {
        IsNotRecommendRule();
        confirmEjects = value;
    }

    [SyncVar(hook = nameof(SetEmergencyMeetings_Hook))]
    private int emergencyMeetings;
    [SerializeField]
    private TextMeshProUGUI emergencyMeetingsText;
    public void SetEmergencyMeetings_Hook(int oldValue, int newValue)
    {
        emergencyMeetingsText.text = newValue.ToString();
        UpdateGameRuleOverview();
    }
    public void OnChangeEmergencyMeetings(bool isPlus)
    {
        IsNotRecommendRule();
        emergencyMeetings = Mathf.Clamp(emergencyMeetings + (isPlus ? 1 : -1), 0, 9);
    }

    [SyncVar(hook = nameof(SetEmergencyMeetingsCooldown_Hook))]
    private int emergencyMeetingsCooldown;
    [SerializeField]
    private TextMeshProUGUI emergencyMeetingsCooldownText;
    public void SetEmergencyMeetingsCooldown_Hook(int oldValue, int newValue)
    {
        emergencyMeetingsCooldownText.text = string.Format("{0}s", newValue);
        UpdateGameRuleOverview();
    }
    public void OnEmergencyMeetingsCooldown(bool isPlus)
    {
        IsNotRecommendRule();
        emergencyMeetingsCooldown = Mathf.Clamp(emergencyMeetingsCooldown + (isPlus ? 5 : -5), 0, 60);
    }

    [SyncVar(hook = nameof(SetMeetingsTime_Hook))]
    private int meetingsTime;
    [SerializeField]
    private TextMeshProUGUI meetingsTimeText;
    public void SetMeetingsTime_Hook(int oldValue, int newValue)
    {
        meetingsTimeText.text = string.Format("{0}s", newValue);
        UpdateGameRuleOverview();
    }
    public void OnMeetingsTime(bool isPlus)
    {
        IsNotRecommendRule();
        meetingsTime = Mathf.Clamp(meetingsTime + (isPlus ? 5 : -5), 0, 120);
    }

    [SyncVar(hook = nameof(SetVoteTime_Hook))]
    private int voteTime;
    [SerializeField]
    private TextMeshProUGUI voteTimeText;
    public void SetVoteTime_Hook(int oldValue, int newValue)
    {
        voteTimeText.text = string.Format("{0}s", newValue);
        UpdateGameRuleOverview();
    }
    public void OnVoteTime(bool isPlus)
    {
        IsNotRecommendRule();
        voteTime = Mathf.Clamp(voteTime + (isPlus ? 5 : -5), 0, 300);
    }

    [SyncVar(hook = nameof(SetAnonymousVotes_Hook))]
    private bool anonymousVotes;
    [SerializeField]
    private Toggle anonymousVotesToggle;
    public void SetAnonymousVotes_Hook(bool oldValue, bool newValue)
    {
        UpdateGameRuleOverview();
    }
    public void OnAnonymousVotesToggle(bool value)
    {
        IsNotRecommendRule();
        anonymousVotes = value;
    }

    [SyncVar(hook = nameof(SetMoveSpeed_Hook))]
    private float moveSpeed;
    [SerializeField]
    private TextMeshProUGUI moveSpeedText;
    public void SetMoveSpeed_Hook(float oldValue, float newValue)
    {
        moveSpeedText.text = string.Format("{0:0.0}x", newValue);
        UpdateGameRuleOverview();
    }
    public void OnMoveSpeed(bool isPlus)
    {
        IsNotRecommendRule();
        moveSpeed = Mathf.Clamp(moveSpeed + (isPlus ? 0.25f : -0.25f), 0.5f, 3f);
    }

    [SyncVar(hook = nameof(SetCrewSight_Hook))]
    private float crewSight;
    [SerializeField]
    private TextMeshProUGUI crewSightText;
    public void SetCrewSight_Hook(float oldValue, float newValue)
    {
        crewSightText.text = string.Format("{0:0.0}x", newValue);
        UpdateGameRuleOverview();
    }
    public void OnCrewSight(bool isPlus)
    {
        IsNotRecommendRule();
        crewSight = Mathf.Clamp(crewSight + (isPlus ? 0.25f : -0.25f), 0.25f, 5f);
    }

    [SyncVar(hook = nameof(SetImposterSight_Hook))]
    private float imposterSight;
    [SerializeField]
    private TextMeshProUGUI imposterSightText;
    public void SetImposterSight_Hook(float oldValue, float newValue)
    {
        imposterSightText.text = string.Format("{0:0.0}x", newValue);
        UpdateGameRuleOverview();
    }
    public void OnImposterSight(bool isPlus)
    {
        IsNotRecommendRule();
        imposterSight = Mathf.Clamp(imposterSight + (isPlus ? 0.25f : -0.25f), 0.25f, 5f);
    }

    [SyncVar(hook = nameof(SetKillCooldown_Hook))]
    private float killCooldown;
    [SerializeField]
    private TextMeshProUGUI killCooldownText;
    public void SetKillCooldown_Hook(float oldValue, float newValue)
    {
        killCooldownText.text = string.Format("{0:0.0}s", newValue);
        UpdateGameRuleOverview();
    }
    public void OnKillCooldown(bool isPlus)
    {
        IsNotRecommendRule();
        killCooldown = Mathf.Clamp(killCooldown + (isPlus ? 2.5f : -2.5f), 10, 60);
    }

    [SyncVar(hook = nameof(SetKillRange_Hook))]
    private EKillRange killRange;
    [SerializeField]
    private TextMeshProUGUI killRangeText;
    public void SetKillRange_Hook(EKillRange oldValue, EKillRange newValue)
    {
        killRangeText.text = newValue.ToString();
        UpdateGameRuleOverview();
    }
    public void OnKillRange(bool isPlus)
    {
        IsNotRecommendRule();
        killRange = (EKillRange)Mathf.Clamp((int)killRange + (isPlus ? 1 : -1), 0, 2);
    }

    [SyncVar(hook = nameof(SetVisualTasks_Hook))]
    private bool visualTasks;
    [SerializeField]
    private Toggle visualTasksToggle;
    public void SetVisualTasks_Hook(bool oldValue, bool newValue)
    {
        UpdateGameRuleOverview();
    }
    public void OnVisualTasksToggle(bool value)
    {
        IsNotRecommendRule();
        visualTasks = value;
    }

    [SyncVar(hook = nameof(SetTaskBarUpdates_Hook))]
    private ETaskBarUdpate taskBarUpdates;
    [SerializeField]
    private TextMeshProUGUI taskBarUpdatesText;
    public void SetTaskBarUpdates_Hook(ETaskBarUdpate oldValue, ETaskBarUdpate newValue)
    {
        taskBarUpdatesText.text = newValue.ToString();
        UpdateGameRuleOverview();
    }
    public void OnTaskBarUpdates(bool isPlus)
    {
        IsNotRecommendRule();
        taskBarUpdates = (ETaskBarUdpate)Mathf.Clamp((int)taskBarUpdates + (isPlus ? 1 : -1), 0, 2);
    }

    [SyncVar(hook = nameof(SetCommonTask_Hook))]
    private int commonTask;
    [SerializeField]
    private TextMeshProUGUI commonTaskText;
    public void SetCommonTask_Hook(int oldValue, int newValue)
    {
        commonTaskText.text = newValue.ToString();
        UpdateGameRuleOverview();
    }
    public void OnCommonTask(bool isPlus)
    {
        IsNotRecommendRule();
        commonTask = Mathf.Clamp(commonTask + (isPlus ? 1 : -1), 0, 2);
    }

    [SyncVar(hook = nameof(SetComplexTask_Hook))]
    private int complexTask;
    [SerializeField]
    private TextMeshProUGUI complexTaskText;
    public void SetComplexTask_Hook(int oldValue, int newValue)
    {
        complexTaskText.text = newValue.ToString();
        UpdateGameRuleOverview();
    }
    public void OnComplexTask(bool isPlus)
    {
        IsNotRecommendRule();
        complexTask = Mathf.Clamp(complexTask + (isPlus ? 1 : -1), 0, 3);
    }

    [SyncVar(hook = nameof(SetSimpleTask_Hook))]
    private int simpleTask;
    [SerializeField]
    private TextMeshProUGUI simpleTaskText;
    public void SetSimpleTask_Hook(int oldValue, int newValue)
    {
        simpleTaskText.text = newValue.ToString();
        UpdateGameRuleOverview();
    }
    public void OnSimpleTask(bool isPlus)
    {
        IsNotRecommendRule();
        simpleTask = Mathf.Clamp(simpleTask + (isPlus ? 1 : -1), 0, 5);
    }

    [SerializeField]
    private TextMeshProUGUI gameRuleOverview;

    [SyncVar(hook = nameof(SetImposterCount_Hook))]
    private int imposterCount;

    public void SetImposterCount_Hook(int oldValue, int newValue)
    {
        UpdateGameRuleOverview();
    }

    public void UpdateGameRuleOverview()
    {
        var manager = NetworkManager.singleton as AmongUsRoomManager;
        StringBuilder sb = new StringBuilder(isRecommendRule ? "추천 설정\n" : "커스텀 설정\n");
        sb.Append("맵: The Skeld\n");
        sb.Append($"#임포스터: {imposterCount}\n");
        sb.Append(string.Format("Confirm Ejects: {0}\n", confirmEjects ? "켜짐" : "꺼짐"));
        sb.Append($"긴급 회의: {emergencyMeetings}\n");
        sb.Append(string.Format("Anonymous Votes: {0}\n", anonymousVotes ? "켜짐" : "꺼짐"));
        sb.Append($"긴급 회의 쿨타임: {emergencyMeetingsCooldown}\n");
        sb.Append($"회의 제한 시간: {meetingsTime}\n");
        sb.Append($"투표 제한 시간: {voteTime}\n");
        sb.Append($"이동 속도: {moveSpeed}\n");
        sb.Append($"크루원 시야: {crewSight}\n");
        sb.Append($"임포스터 시야: {imposterSight}\n");
        sb.Append($"킬 쿨타임: {killCooldown}\n");
        sb.Append($"킬 범위: {killRange}\n");
        sb.Append($"Task Bar Updates: {taskBarUpdates}\n");
        sb.Append(string.Format("Visual Tasks: {0}\n", visualTasks ? "켜짐" : "꺼짐"));
        sb.Append($"공통 임무: {commonTask}\n");
        sb.Append($"복잡한 임무: {complexTask}\n");
        sb.Append($"간단한 임무: {simpleTask}\n");
        gameRuleOverview.text = sb.ToString();
    }

    private void SetRecommendGameRule()
    {
        isRecommendRule = true;
        confirmEjects = true;
        emergencyMeetings = 1;
        emergencyMeetingsCooldown = 15;

#if UNITY_EDITOR
        meetingsTime = 0;
#else
        meetingsTime = 15;
#endif
        voteTime = 120;
        moveSpeed = 1f;
        crewSight = 1f;
        imposterSight = 1.5f;
        killCooldown = 45f;
        killRange = EKillRange.Normal;
        visualTasks = true;
        commonTask = 1;
        complexTask = 1;
        simpleTask = 2;
    }

    private GameRuleData gameRuleData;
    public GameRuleData GameRuleData
    {
        get
        {
            gameRuleData.anonymousVotes = anonymousVotes;
            gameRuleData.commonTask = commonTask;
            gameRuleData.complexTask = complexTask;
            gameRuleData.crewSight = crewSight;
            gameRuleData.emergencyMeetings = emergencyMeetings;
            gameRuleData.emergencyMeetingsCooldown = emergencyMeetingsCooldown;
            gameRuleData.imposterSight = imposterSight;
            gameRuleData.killCooldown = killCooldown;
            gameRuleData.killRange = killRange;
            gameRuleData.meetingsTime = meetingsTime;
            gameRuleData.moveSpeed = moveSpeed;
            gameRuleData.simpleTask = simpleTask;
            gameRuleData.taskBarUpdates = taskBarUpdates;
            gameRuleData.visualTasks = visualTasks;
            gameRuleData.voteTime = voteTime;

            return gameRuleData;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
        {
            var manager = NetworkManager.singleton as AmongUsRoomManager;
            imposterCount = manager.ImposterCount;
            anonymousVotes = false;
            taskBarUpdates = ETaskBarUdpate.Always;

            SetRecommendGameRule();
        }
    }

}
