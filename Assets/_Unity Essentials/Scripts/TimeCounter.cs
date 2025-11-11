using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Text.RegularExpressions;

namespace UnityEssentials
{
    /// <summary>
    /// Timer de UI usando TextMeshProUGUI.
    /// Contagem regressiva a partir de `startMinutes` (padrão 3.0) e exibe no formato mm:ss.
    /// Agora aceita valores fracionados de minutos (ex.: 2.5).
    /// Métodos públicos: StartTimer, PauseTimer, ResetTimer, SetMinutes.
    /// Evento: OnTimerFinished.
    /// </summary>
    public class TimeCounter : MonoBehaviour
    {
        [Header("Configurações")]
        [Tooltip("Componente TextMeshProUGUI que exibirá o tempo.")]
        [SerializeField] private TextMeshProUGUI timerText;

        [Tooltip("Tempo inicial em minutos (aceita fracionado, ex.: 2.5).")]
        [SerializeField] private float startMinutes = 3f;

        [Tooltip("Iniciar automaticamente ao Awake.")]
        [SerializeField] private bool autoStart = true;

        [Header("Eventos")]
        [SerializeField] private UnityEvent OnTimerFinished;

        private float remainingSeconds;
        public float RemainingSeconds => remainingSeconds;
        private bool running;
        private bool finishedTriggered;

        // Template original do texto (cabeçalho) que pode conter {Y} ou Y como placeholder
        private string headerTemplate;

        private void Awake()
        {
            if (timerText == null)
            {
                timerText = GetComponentInChildren<TextMeshProUGUI>();
            }

            // Captura o texto atual do componente como template (preserva o cabeçalho)
            headerTemplate = timerText != null ? timerText.text : string.Empty;

            remainingSeconds = startMinutes * 60f;
            UpdateDisplay(remainingSeconds);

            if (autoStart)
                StartTimer();
        }

        private void Update()
        {
            if (!running) return;
            if (remainingSeconds <= 0f) return;

            remainingSeconds -= Time.deltaTime;
            if (remainingSeconds <= 0f)
            {
                remainingSeconds = 0f;
                running = false;
                TriggerFinished();
            }

            UpdateDisplay(remainingSeconds);
        }

        private void UpdateDisplay(float seconds)
        {
            int minutesPart = Mathf.FloorToInt(seconds / 60f);
            int secondsPart = Mathf.FloorToInt(seconds % 60f);
            string formatted = string.Format("{0:D2}:{1:D2}", minutesPart, secondsPart);

            if (timerText == null)
                return;

            // Se o template estiver vazio, apenas mostra o tempo
            if (string.IsNullOrEmpty(headerTemplate))
            {
                timerText.text = formatted;
                return;
            }

            // Prioriza {Y} como placeholder explícito
            if (headerTemplate.Contains("{Y}"))
            {
                timerText.text = headerTemplate.Replace("{Y}", formatted);
                return;
            }

            // Substitui apenas a primeira ocorrência de Y como token separado (ex.: "Header Y restante")
            bool replaced = false;
            string pattern = @"\bY\b";
            string result = Regex.Replace(headerTemplate, pattern, m =>
            {
                if (replaced) return m.Value;
                replaced = true;
                return formatted;
            });

            if (replaced)
            {
                timerText.text = result;
                return;
            }

            // Se não encontrou placeholder, anexa o tempo ao final preservando o cabeçalho
            timerText.text = string.IsNullOrWhiteSpace(headerTemplate)
                ? formatted
                : $"{headerTemplate} {formatted}";
        }

        private void TriggerFinished()
        {
            if (finishedTriggered) return;
            finishedTriggered = true;
            OnTimerFinished?.Invoke();
        }

        // API pública
        public void StartTimer()
        {
            if (remainingSeconds <= 0f)
            {
                // Reinicia se já terminou
                ResetTimer();
            }
            running = true;
            finishedTriggered = false;
        }

        public void PauseTimer()
        {
            running = false;
        }

        public void ResetTimer()
        {
            remainingSeconds = startMinutes * 60f;
            running = false;
            finishedTriggered = false;
            UpdateDisplay(remainingSeconds);
        }

        // Aceita valores fracionados de minutos
        public void SetMinutes(float minutes, bool restart = false)
        {
            startMinutes = Mathf.Max(0f, minutes);
            remainingSeconds = startMinutes * 60f;
            finishedTriggered = false;
            UpdateDisplay(remainingSeconds);
            if (restart) StartTimer();
        }

        // Sobrecarga para compatibilidade com chamadas que usam int
        public void SetMinutes(int minutes, bool restart = false)
        {
            SetMinutes((float)minutes, restart);
        }

        // Expor tempo restante em segundos (leitura)
        public float GetRemainingSeconds() => remainingSeconds;

        // Expor estado
        public bool IsRunning() => running;
    }
}