# �g����
C#�ŏ����ꂽ�R�}���h���C���A�v���P�[�V�����ł��B
Visual Studio 2022�Ńr���h���Ă��������B
�r���h��A�ȉ��̏����ň������w�肵�Ă��������B

```
$ ./TestBenchTemplateWriter.exe �o�͐�t�H���_�̃t���p�X <���̓t�H���_�̃t���p�X1> [<���̓t�H���_�̃t���p�X2> ...]
```

�o�͐�t�H���_�ɂ́A���̓t�H���_��SystemVerilog�t�@�C��(�g���qsv)�̃e�X�g�x���`���o�͂��܂��B
���̓t�H���_�̃T�u�t�H���_���ċA�I�ɒT�����܂��B
�o�͐�t�H���_�ɁA���̓t�H���_�Ɠ����̃t�H���_���쐬���āA���̓t�H���_�̍\����ۂ��܂��B
�Ⴆ��```C:\Users\wolfm\Documents\CAD\SIMPLE\SIMPLE_OoO\TOPLEVEL```����̓t�H���_�Ƃ���ƁA
�o�͐�t�H���_��```TOPLEVEL```�Ƃ������O�̃t�H���_���쐬���āA���̓t�H���_�̍\����ۂ��e�X�g�x���`���쐬���Ă���܂��B
�Ȃ��A�e�X�g�x���`��SystemVerilog�ł��̂ŁA�V�����e�X�g�x���`��ǉ�����Ƃ��ɁA
HDL Version��SystemVerilog 2005�ɐݒ肵�Ă��������B

����ɁA�Ώۃ��W���[���̐錾�����͈ȉ��̏����ɏ]���Ă���K�v������܂��B

- module�錾������s���邱��
- �S�Ă̓��o�͐���1�s���g������
- input logic��output logic�̂悤�Ȑ錾��S�Ă̓��o�͐��ɑ΂��čs������(�ȗ����Ă͂����Ȃ�)

�������`���̐錾�̗�������܂��B

```
module sampleModule(
    // �R�����g�͖�������܂�
    input logic simpleLogic,
    input logic [2:0] bus1,
    input logic [51:0] bus2,
    // �������z��ɂ��Ή����Ă��܂�
    output logic [51:0] ary1 [7:0]
);
```

���̓��͂ɑ΂���o�͈͂ȉ��̂悤�Ȃ��̂ł�

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