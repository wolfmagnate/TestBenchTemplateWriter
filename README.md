# 使い方
C#で書かれたコマンドラインアプリケーションです。
Visual Studio 2022でビルドしてください。
ビルド後、以下の書式で引数を指定してください。

```
$ ./TestBenchTemplateWriter.exe 出力先フォルダのフルパス <入力フォルダのフルパス1> [<入力フォルダのフルパス2> ...]
```

出力先フォルダには、入力フォルダのSystemVerilogファイル(拡張子sv)のテストベンチを出力します。
入力フォルダのサブフォルダも再帰的に探索します。
出力先フォルダに、入力フォルダと同名のフォルダを作成して、入力フォルダの構造を保ちます。
例えば```C:\Users\wolfm\Documents\CAD\SIMPLE\SIMPLE_OoO\TOPLEVEL```を入力フォルダとすると、
出力先フォルダに```TOPLEVEL```という名前のフォルダを作成して、入力フォルダの構造を保ちつつテストベンチを作成してくれます。
なお、テストベンチもSystemVerilogですので、新しくテストベンチを追加するときに、
HDL VersionをSystemVerilog 2005に設定してください。

さらに、対象モジュールの宣言部分は以下の書式に従っている必要があります。

- module宣言から改行すること
- 全ての入出力線に1行ずつ使うこと
- input logicやoutput logicのような宣言を全ての入出力線に対して行うこと(省略してはいけない)

正しい形式の宣言の例を示します。

```
module sampleModule(
    // コメントは無視されます
    input logic simpleLogic,
    input logic [2:0] bus1,
    input logic [51:0] bus2,
    // 多次元配列にも対応しています
    output logic [51:0] ary1 [7:0]
);
```

この入力に対する出力は以下のようなものです

```
module sampleModule_test();
    logic simpleLogic;
    logic [2:0] bus1;
    logic [51:0] bus2;
    logic [51:0] ary1 [7:0];

    sampleModule sampleModule_inst(simpleLogic,bus1,bus2,ary1);

    initial begin
        // TODO
    end

    always begin
		// TODO
	end
endmodule
```