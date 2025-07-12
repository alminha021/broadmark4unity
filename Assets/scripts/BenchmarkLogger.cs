using UnityEngine;
using System.IO;
using System.Globalization;

public class BenchmarkLogger : MonoBehaviour
{
    [Header("Duração da coleta em segundos")]
    [Tooltip("Tempo que o benchmark vai rodar antes de salvar (em segundos)")]
    // fixado a duraçao do treste em 30seg , para freefall e randomgravity recomendo alterar, pois eles alcancam equilibrio
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
            Debug.Log(" Benchmark finalizado e salvo.");
            enabled = false;
        }
    }

    void SalvarResultado()
    {
        // Coleta dados calculados
        float frameTimeMs = (tempoAcumulado / frameCount) * 1000f;
        float fps = 1000f / frameTimeMs;
        long memoriaGC_MB = System.GC.GetTotalMemory(false) / (1024 * 1024);

        // Pega configs atuais do BenchmarkConfig
        string algoritmoNome = BenchmarkConfig.Instance.algoritmo;
        string cenarioNome = BenchmarkConfig.Instance.scenario.ToString();
        int numeroDeObjetos = BenchmarkConfig.Instance.numeroDeObjetos;
        //local, ele fala no log onde salva, ams geralmente em users/profile/projectname/assets
        string caminho = Path.Combine(Application.dataPath, "benchmark_dataset.csv");
        bool arquivoExiste = File.Exists(caminho);
        //Organizacao de como fica no dataset
        using (StreamWriter sw = new StreamWriter(caminho, true))
        {
            if (!arquivoExiste)
            {
                sw.WriteLine("Algoritmo,Cenario,NumObjetos,TempoTotal_s,FPS_Medio,FrameTime_ms,Memoria_MB");
            }

            string linha = string.Format(CultureInfo.InvariantCulture,
                "{0},{1},{2},{3:F2},{4:F2},{5:F2},{6}",
                algoritmoNome, cenarioNome, numeroDeObjetos,
                tempoDecorrido, fps, frameTimeMs, memoriaGC_MB);

            sw.WriteLine(linha);
        }

        Debug.Log($"Resultado salvo em: {caminho}");
    }
}
