using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

public class Program
{
    static void Main(string[] args)
    {
        // コマンドライン引数が2つ未満の場合、使い方を表示してプログラムを終了
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: <output directory> <input directory1> [<input directory2> ...]");
            return;
        }

        // 出力ディレクトリを取得
        string outputDir = args[0];

        // 入力ディレクトリを1つずつ処理
        for (int i = 1; i < args.Length; i++)
        {
            string inputDir = args[i];

            // 入力ディレクトリ内のすべての".sv"ファイルを再帰的に取得
            foreach (string file in Directory.GetFiles(inputDir, "*.sv", SearchOption.AllDirectories))
            {
                // ファイルの内容を読み込み
                string content = File.ReadAllText(file);

                // テストベンチを生成
                string testbench = GenerateTestbench(content);

                // 新しいディレクトリとファイルパスを計算
                string relativePath = Path.GetRelativePath(inputDir, file);
                string newDirPath = Path.Combine(outputDir, Path.GetFileName(inputDir));
                string newFilePath = Path.Combine(newDirPath, relativePath);

                // 新しいディレクトリを作成
                string newDir = Path.GetDirectoryName(newFilePath);
                Directory.CreateDirectory(newDir);

                // 新しいファイル名を作成してテストベンチを書き込む
                string newFile = Path.Combine(newDir, Path.GetFileNameWithoutExtension(file) + "_test.sv");
                File.WriteAllText(newFile, testbench);
            }
        }
    }

    static string GenerateTestbench(string content)
    {
        // モジュール名とパラメータを抽出
        Match match = Regex.Match(content, @"module\s+(\w+)(.*?);", RegexOptions.Singleline);

        // モジュール名とパラメータが見つからない場合、nullを返す
        if (!match.Success)
            return null;

        // モジュール名とパラメータを取得
        string moduleName = match.Groups[1].Value;
        string parameters = match.Groups[2].Value.Trim().Trim('(', ')');

        // パラメータを行ごとに分割
        string[] lines = parameters
            .Split(new[] { ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            // コメントを削除
            .Where(x => !string.IsNullOrEmpty(x) && !x.StartsWith("//"))
            .ToArray();

        // テストベンチのヘッダを作成
        string testbench = $"`timescale 1ns/1ns\nmodule {moduleName}_test();\n";

        // インスタンス化の際のパラメータを格納するリスト
        List<string> instanceParams = new List<string>();
        // 各行を処理
        for (int i = 0; i < lines.Length; i++)
        {
            // 行の両端の空白を削除し、行内の要素を分割
            string line = lines[i].Trim().Trim('\r', '\n');
            string[] parts = line.Split(new[] { ' ', '[', ']', ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // コメント行はスキップ
            if (line.StartsWith("//")) { continue; }

            // 変数名を追跡するためのインデックス
            int j = 1;

            // logic宣言の開始
            string logicDeclaration = "    logic ";

            // 複数次元配列を処理するためのループ
            while (j < parts.Length)
            {
                // inputやoutputが含まれている場合はスキップ
                if (parts[j].Contains("input") || parts[j].Contains("output"))
                {
                    j++;
                    continue;
                }

                // 7要素の場合、二次元配列
                if (parts.Length == 7)
                {
                    logicDeclaration += $"[{parts[j + 1]}:{parts[j + 2]}] {parts[j + 3]} [{parts[j + 4]}:{parts[j + 5]}]";
                    instanceParams.Add(parts[j + 3]);
                    break;
                }
                // 5要素の場合、一次元配列
                else if (parts.Length == 5)
                {
                    logicDeclaration += $"[{parts[j + 1]}:{parts[j + 2]}] {parts[j + 3]}";
                    instanceParams.Add(parts[j + 3]);
                    break;
                }
                // それ以外の場合、通常の変数
                else
                {
                    logicDeclaration += parts[j + 1];
                    instanceParams.Add(parts[j + 1]);
                    break;
                }
            }

            // logic宣言をテストベンチに追加
            testbench += logicDeclaration + ";\n";
        }

        // インスタンス化部分をテストベンチに追加
        testbench += $"\n    {moduleName} i1(";
        testbench += string.Join(",", instanceParams);
        testbench += ");\n\n    initial begin\n        // TODO\n    end\n\n    always begin\n        // TODO\n    end\nendmodule\n";

        // テストベンチを返す
        return testbench;
    }
}