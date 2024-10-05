# RE4-UHD-BIN-TOOL
Extract and repack RE4 UHD/PS4/NS .BIN/.TPL files

**Translate from Portuguese Brazil**

Programa destinado a extrair e reempacotar os arquivos .bin/.tpl do RE4 UHD/PS4/NS;
<br>Nota1: Não suportado o campo de Morph (não foi implementada essa funcionalidade no programa);
<br>Nota2: A versão de UHD é um executável e a versão de Ps4/Ns é outro executável;

## Updates

**Update: B.1.1.0**
<br>Adicionado suporte para as versões de PS4 e NS;

**Update: B.1.0.8**
<br>Adicionado compatibilidade com mais arquivos BIN/TPL;
<br>Adicionado suporte para cores de vértices para os arquivos OBJ, para ativar essa função, você deve definir a variável "UseVertexColor" como "True" no arquivo "idxuhdbin". Nota: ao extrair o arquivo, essa variável vai ser sempre "False".
<br>Agora, ao fazer repack com OBJ/SMD o arquivo "idxuhdtpl" será ignorado, para usá-lo, você deve passar esse arquivo como segundo parâmetro.

**Update: B.1.0.07**
<br>Agora, ao extrair o arquivo .bin as "normals" serão normalizadas, em vez de ser dividida por um valor padrão, então agora é possível extrair os arquivos .bin gerados pela tool do percia sem erros.
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
<br> * No repack ao ler as dimensões das imagens, agora será lido somente uma vez cada imagens, em vez de várias vezes como era feito na versão anterior.
<br> * Nota: essa versão é compatível com os arquivos da versão anterior.

## JADERLINK_RE4_\*\*_BIN_TOOL.exe

Programa responsável por extrair e recompilar os arquivos .bin/.tpl;
<br> Segue abaixo os "inputs" e "outputs" do programa:
<br>Nota: o programa pode receber um ou dois arquivos como parâmetro;

* **JADERLINK_RE4_\*\*_BIN_TOOL.exe "file.BIN"**
    <br>Extrai o arquivo bin vai gerar os arquivos: file.obj, file.smd, file.idxmaterial e file.idxuhdbin;
    <br>Caso na pasta tenha um arquivo .tpl de mesmo nome do bin, será considerado como se tivesse passado o arquivo também como parâmetro;
* **JADERLINK_RE4_\*\*_BIN_TOOL.exe "file.TPL"**
    <br>Extrai o arquivo tpl vai gerar o arquivo: file.idxuhdtpl;
* **JADERLINK_RE4_\*\*_BIN_TOOL.exe "file.BIN" "file.TPL"**
    <br>Ira gera os arquivos citados anteriormente mais o arquivo: file.mtl;
* **JADERLINK_RE4_\*\*_BIN_TOOL.exe "file.OBJ"**
    <br>Faz repack do arquivo .bin, requisita na mesma pasta o arquivo .idxuhdbin de mesmo nome e o arquivo .mtl de mesmo nome; <del>e opcionalmente o arquivo .idxuhdtpl</del>
    <br>Nota: você pode passar como segundo parâmetro o arquivo .mtl ou .idxuhdtpl/.tpl, o resultado da operação é o mesmo citado acima;
    <br>Nota2: as operações com o .mtl envolvido requisitam que na mesma pasta do mtl tenha as texturas na qual ele faz referência;
    <br>Nota3: a partir da versão B.1.0.8, para usar o arquivo .idxuhdtpl/.tpl, você deve passá-lo como segundo parâmetro.
* **JADERLINK_RE4_\*\*_BIN_TOOL.exe "file.OBJ" "file.idxmaterial"**
    <br>Faz repack do arquivo .bin, usando os arquivo .obj e .idxmaterial, requisita somente na mesma pasta o arquivo .idxuhdbin;
* **JADERLINK_RE4_\*\*_BIN_TOOL.exe "file.SMD"**
    <br> Mesma explicação que do arquivo .obj, so que agora fazendo o repack usando o arquivo .smd;
* **JADERLINK_RE4_\*\*_BIN_TOOL.exe "file.SMD" "file.idxmaterial"**
    <br>O mesmo que expliquei acima.
* **JADERLINK_RE4_\*\*_BIN_TOOL.exe "file.MTL"**
    <br>"Extrai" o arquivo .mtl cria os arquivos: File.Repack.idxmaterial e File.Repack.idxuhdtpl
    <br>Nota: você pode passar como segundo parâmetro o arquivo .tpl/.idxuhdtpl, no qual ele vai usar como referência para ordenar as texturas no tpl;
    <br>Nota2: as operações com o .mtl envolvido requisitam que na mesma pasta do mtl tenha as texturas na qual ele faz referência;
* **JADERLINK_RE4_\*\*_BIN_TOOL.exe "file.tpl" "file.idxmaterial"**
    <br> Cria o arquivo .mtl;
* **JADERLINK_RE4_\*\*_BIN_TOOL.exe "file.idxuhdtpl" "file.idxmaterial"**
    <br> Também cria o arquivo .mtl;

## Explicação para que serve cada arquivo:

* .BIN: esse é o modelo 3d do jogo.
* .TPL: esse é o arquivo que guarda a referência das texturas usadas no modelo.
* .OBJ: modelo 3d que pode ser editado em um editor 3d;
* .MTL: arquivo que contém os materiais para serem carregados no editor 3d;
* .SMD: (StudioModel Data) modelo 3d que pode ser editado em um editor 3d (com suporte para bones);
* .IDXUHDBIN: arquivo necessário para recompilar o arquivo .bin
* .IDXUHDTPL: é a versão editável do arquivo .TPL;
* .IDXMATERIAL: é o arquivo que contém os materiais presentes no .bin (pode ser editado);

## Arquivo .IDXUHDBIN
Arquivo com configurações adicionais para o repack
<br>veja sobre aqui: [RE4 UHD BIN TOOL - Documentação: IDXUHDBIN](https://jaderlink.blogspot.com/2024/08/RE4-UHD-BIN-TOOL-IDXUHDBIN.html)

## Arquivo .IDXUHDTPL
Pode ser usado para editar/criar arquivos .tpl
<br>veja sobre aqui: [RE4 UHD BIN TOOL - Documentação: IDXUHDTPL](https://jaderlink.blogspot.com/2023/11/RE4-UHD-BIN-TOOL-IDXUHDTPL.html)

## Arquivo .IDXMATERIAL
Pode ser usado para editar os materiais do bin sem usar o arquivo .mtl
<br>veja sobre isso aqui: [RE4 UHD BIN TOOL - Documentação: IDXMATERIAL](https://jaderlink.blogspot.com/2023/11/RE4-UHD-BIN-TOOL-IDXMATERIAL.html)

## Arquivo .MTL
É usado para carregar as texturas no editor 3d, também pode ser usado para recompilar os materiais, ou diretamente o arquivo .bin; 
<br>veja sobre isso aqui: [RE4 UHD BIN TOOL - Documentação: MTL](https://jaderlink.blogspot.com/2023/11/RE4-UHD-BIN-TOOL-MTL.html)

## Ordem dos bones no arquivo .SMD

Para arrumar a ordem dos ids dos bones nos arquivos smd, depois de serem exportados do blender ou outro software de edição de modelos, usar o programa: GC_GC_Skeleton_Changer.exe (procure o programa no fórum do re4, remod)

## Carregando as texturas no arquivo .SMD

No blender para carregar o modelo .SMD com as texturas, em um novo "projeto", importe primeiro o arquivo .obj para ele carregar as texturas, delete o modelo do .obj importado, agora importe o modelo .smd, agora ele será carregado com as texturas.
<br>Lembrando também que as texturas devem estar na pasta com o nome de seu arquivo .PACK e essa pasta deve estar ao lado do arquivo .mtl;

## Código de terceiro:

[ObjLoader by chrisjansson](https://github.com/chrisjansson/ObjLoader):
Encontra-se em "RE4_UHD_BIN_TOOL\\CjClutter.ObjLoader.Loader", código modificado, as modificações podem ser vistas aqui: [link](https://github.com/JADERLINK/ObjLoader).

**At.te: JADERLINK**
<br>Thanks to \"mariokart64n\" and \"CodeMan02Fr\"
<br>Material information by \"Albert\"
<br>2024-10-06
