using UnityEngine;
using System.IO;
using System.Globalization;
public class BenchmarkLogger : MonoBehaviour
{
    [Header("Configuração do Benchmark")]
    [Tooltip("Ex: KDTree, Tracy, BruteForce")]
    public string algoritmoNome = "KDTree";

    [Tooltip("Ex: 200, 400, 800, 1600")]
    public int numeroDeObjetos = 200;

    [Tooltip("Ex: Brownian, FreeFall, RandomGravity, RotatingGravity, Hurricane")]
    public string cenarioNome = "Brownian";

    [Tooltip("Duração da coleta em segundos")]
    public float duracaoEmSegundos = 30f;

    private int frameCount = 0;
    private float tempoAcumulado = 0f;
    private float tempoDecorrido = 0f;

    void Update()
    {
        tempoDecorrido += Time.unscaledDeltaTime;
        tempoAcumulado += Time.unscaledDeltaTime;
        frameCount++;

        if (tempoDecorrido >= duracaoEmSegundos)
        {
            SalvarResultado();
            Debug.Log("✅ Benchmark finalizado e salvo.");
            enabled = false;
        }
    }

    void SalvarResultado()
    {
        float frameTimeMs = (tempoAcumulado / frameCount) * 1000f;
        float fps = 1000f / frameTimeMs;
        long memoriaGC_MB = System.GC.GetTotalMemory(false) / (1024 * 1024);

        string caminho = Path.Combine(Application.dataPath, "benchmark_dataset.csv");

        bool arquivoExiste = File.Exists(caminho);

        using (StreamWriter sw = new StreamWriter(caminho, true))
        {
            if (!arquivoExiste)
            {
                sw.WriteLine("Algoritmo,Cenario,NumObjetos,TempoTotal_s,FPS_Medio,FrameTime_ms,Memoria_MB");
            }

                    string linha = string.Format(CultureInfo.InvariantCulture,
            "{0},{1},{2},{3:F2},{4:F2},{5:F2},{6}",algoritmoNome, cenarioNome, numeroDeObjetos,tempoDecorrido, fps, frameTimeMs, memoriaGC_MB);
            sw.WriteLine(linha);
        }

        Debug.Log($"📁 Resultado salvo em: {caminho}");
    }
}
