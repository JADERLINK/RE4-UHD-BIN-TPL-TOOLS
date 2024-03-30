# RE4-UHD-BIN-TOOL
Extract and repack RE4 UHD .BIN/.TPL files

**Translate from Portuguese Brazil**

Programa destinado a extrair e reempacotar os arquivos .bin/.tpl do re4 Uhd;
<br>Nota1: o programa não suporta cores por vértices (.obj e .smd não tem suporte para isso);
<br>Nota2: Não suportado o campo de Morph (não foi implementado essa funcionalidade no programa);

**Update: B.1.0.07**
<br>Agora, ao extrair o arquivo .bin as "normals" serão normalizadas em vez de ser dividido por um valor padrão, então agora é possível extrair os arquivos .bin gerados pela tool do percia sem erros.
<br> Ao fazer repack as normals dos arquivos .obj e .smd serão normalizadas para evitar erros.
<br> O programa, ao gerar os arquivos .obj e .smd, não terá mais os zeros não significativos dos números, mudança feita para gerar arquivos menores.

**Update: B.1.0.0.6**
<br>Arrumado bug ao carregar o arquivo .idxmaterial;

**Update: B.1.0.0.5**
<br>Agora o programa é compatível em extrair e criar arquivos .BIN acima do limite de vértices;
<br>Atenção: Os .BIN com vértices acima do limite só funcionam corretamente se eles forem usados dentro de arquivos Scenario .SMD;
<br>Em outras situações, o limite ainda é valido;

**Update: B.1.0.0.4**
<br> Corrigido bug no qual o arquivo MTL com PACK_ID com IDs que continham letras, as letras não eram consideradas.

**Update: B.1.0.0.3**
<br> Corrido erro, ao ter material sem a textura principal "map_Kd", será preenchido como Pack ID 00000000 e Texture ID 000;
<br> Agora, caso a quantidade de vértices for superior ao limite do arquivo, o programa vai avisar. (Não será criado o arquivo BIN);

**Update: B.1.0.0.2**
<br> Corrigido bug que deformava a malha do modelo 3d, estava sendo criado faces do tipo "quad" de maneira errada; 

**Update: B.1.0.0.1**
<br> * correções de bugs: ao gerar o arquivo .mtl com um .tpl que não é o correto para o .bin, estava crashando o programa, isso foi corrigido, agora caso faltar referência no arquivo .tpl, no arquivo .mtl será preenchido "00000000/0000.null", isso significa, que o .tpl fornecido não é do .bin em questão;
<br> * No repack ao ler as dimensões das imagens, agora será lido somente uma vez cada imagens, em vez de vária vezes como era feito na versão anterior.
<br> *Nota: essa versão é compatível com os arquivos da versão anterior.

## JADERLINK_UHD_BIN_TOOL.exe

Programa responsavel por extrair e recompilar os arquivos .bin/.tpl;
<br> Segue abaixo os "inputs" e "outputs" do programa:
<br>Nota: o programa pode receber um ou dois arquivos como parâmetro;

* **JADERLINK_UHD_BIN_TOOL.exe "file.BIN"**
    <br>Extrai o arquivo bin ira gerar os arquivos: file.obj, file.smd, file.idxmaterial e file.idxuhdbin;
    <br>Caso na pasta tiver um arquivo .tpl de mesmo nome do bin, será considerado como se tivesse passado arquivo também como parâmetro;
* **JADERLINK_UHD_BIN_TOOL.exe "file.TPL"**
    <br>Extrai o arquivo tpl ira gerar o arquivo: file.idxuhdtpl;
* **JADERLINK_UHD_BIN_TOOL.exe "file.BIN" "file.TPL"**
    <br>Ira gera os arquivos citados anteriormente mais o arquivo: file.mtl;
* **JADERLINK_UHD_BIN_TOOL.exe "file.OBJ"**
    <br>Faz repack do arquivo .bin, requisita na mesma pasta o arquivo .idxuhdbin de mesmo nome, o arquivo .mtl de mesmo nome e opcionalmente o arquivo .idxuhdtpl
    <br>Nota: você pode passar como segundo parâmetro o arquivo .mtl ou .idxuhdtpl/.tpl, o resultado da operação é o mesmo citado acima;
    <br>Nota2: as operações com o .mtl envolvido requisita que na mesma pasta do mtl tenha as texturas na qual ele faz referência;
* **JADERLINK_UHD_BIN_TOOL.exe "file.OBJ" "file.idxmaterial"**
    <br>Faz repack do arquivo .bin, usado os arquivo .obj e .idxmaterial, requisita somente na mesma pasta o arquivo .idxuhdbin;
* **JADERLINK_UHD_BIN_TOOL.exe "file.SMD"**
    <br> Mesma explicação que do arquivo .obj, so que agora fazendo o repack usando o arquivo .smd;
* **JADERLINK_UHD_BIN_TOOL.exe "file.SMD" "file.idxmaterial"**
    <br>O mesmo que expliquei acima.
* **JADERLINK_UHD_BIN_TOOL.exe "file.MTL"**
    <br>"Extrai" o arquivo .mtl cria os arquivos: File.Repack.idxmaterial e File.Repack.idxuhdtpl
    <br>Nota: você pode passar como segundo parâmetro o arquivo .tpl/.idxuhdtpl, no qual ele vai usar como referência para ordenar as texturas no tpl;
    <br>Nota2: as operações com o .mtl envolvido requisita que na mesma pasta do mtl tenha as texturas na qual ele faz referência;
* **JADERLINK_UHD_BIN_TOOL.exe "file.tpl" "file.idxmaterial"**
    <br> Cria o arquivo .mtl;
* **JADERLINK_UHD_BIN_TOOL.exe "file.idxuhdtpl" "file.idxmaterial"**
    <br> Também cria o arquivo .mtl;

## Explicação para que serve cada arquivo:

* .BIN: esse é o modelo 3d do jogo.
* .TPL: esse é arquivo que guarda a referencia das texturas usadas no modelo.
* .OBJ: modelo 3d que pode ser editado em um editor 3d;
* .MTL: arquivo que contem os matérias para serem carregados no editor 3d;
* .SMD: (StudioModel Data) modelo 3d que pode ser editado em um editor 3d (com suporte para bones);
* .IDXUHDBIN: arquivo necessário para recompilar o arquivo .bin
* .IDXUHDTPL: é verão editável do arquivo .TPL;
* .IDXMATERIAL: é o arquivo que contem os materiais presente no .bin (pode ser editado);

## Arquivo .IDXUHDBIN
(irei colocar futuramente informações sobre esse arquivo aqui)

## Arquivo .IDXUHDTPL
Pode ser usado para editar/criar arquivos .tpl
<br>veja sobre aqui: [RE4 UHD BIN TOOL - Documentação: IDXUHDTPL](https://jaderlink.blogspot.com/2023/11/RE4-UHD-BIN-TOOL-IDXUHDTPL.html)

## Arquivo .IDXMATERIAL
Pode ser usado para editar os materias do bin sem usar o arquivo .mtl
<br>veja sobre aqui: [RE4 UHD BIN TOOL - Documentação: IDXMATERIAL](https://jaderlink.blogspot.com/2023/11/RE4-UHD-BIN-TOOL-IDXMATERIAL.html)

## Arquivo .MTL
è usado para carregar as texturas no editor 3d, tabem pode ser usado para recompilar os materiais, ou diretamente o arquivo .bin; 
<br>veja sobre aqui: [RE4 UHD BIN TOOL - Documentação: MTL](https://jaderlink.blogspot.com/2023/11/RE4-UHD-BIN-TOOL-MTL.html)

## Ordem dos bones no arquivo .SMD

Para arrumar a ordem dos ids dos bones nos arquivos smd, depois de serem exportados do blender ou outro software de edição de modelos usar o programa: GC_GC_Skeleton_Changer.exe (procure o programa no fórum do re4)

## Carregando as texturas no arquivo .SMD

No blender para carregar o modelo .SMD com as texturas, em um novo "projeto", importe primeiro o arquivo .obj para ele carregar as texturas, delete o modelo do .obj importado, agora importe o modelo .smd, agora ele será carregado com as texturas.
<br>Lembrando também que tem que as texturas devem estar na pasta com o nome de seu arquivo .PACK e essa pasta deve esta ao lado do arquivo .mtl;

## Código de terceiro:

[ObjLoader by chrisjansson](https://github.com/chrisjansson/ObjLoader):
Encontra-se no JADERLINK_UHD_BIN_TOOL, código modificado, as modificações podem ser vistas aqui: [link](https://github.com/JADERLINK/ObjLoader).

**At.te: JADERLINK**
<br>**2024-03-30**
