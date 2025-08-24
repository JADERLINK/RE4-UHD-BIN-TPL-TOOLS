# RE4-UHD-BIN-TPL-TOOLS
Extract and repack BIN/TPL files for RE4 OG UHD/PS4/NS/X360/PS3;

**Translate from Portuguese Brazil**

Programa destinado a extrair e reempacotar os arquivos BIN/TPL do RE4 OG UHD/PS4/NS/X360/PS3;
<br>Nota1: Não suportado o campo de Morph (não foi implementada essa funcionalidade no programa);
<br>Nota2: Agora Morph são extraídos, porém, ainda não tem como fazer o repack deles.
<br>Nota3: A versão de UHD é um executável, a versão de PS4/NS é outro executável, e a versão de X360/PS3 também é outro executável;

## Updates

**Update: V.1.4.0**
<br>Foi trocado os arquivos 'idxuhdbin' e 'idxuhdbinbig' para 'idxuubin' no qual seu conteúdo agora é todo em decimal, e mais fácil de editar.
<br>Os Morphs são extraídos agora, porém ainda não tem como fazer repack deles.
<br>Feitos melhorias no código.
<br>Agora todas as 3 tools têm os mesmos conjunto de dados, podendo ter o mesmo modelo 3d como destino para todas as versões suportadas nesse repositório, lembrando, claro, que cada versão tem um arquivo pack diferente. Cada versão tem que fazer o repack com o executável correto.
<br>Passando somente o arquivo 'idxuubin' para o executável, vai ser criado um bin valido sem modelagem 3d (invisível).

**Update: V.1.3.2**
<br>Adicionado suporte ao Linux via mono e seu sistema de diretório;
<br>Adicionado os campos faltantes do TPL, agora tem como extrair e fazer repack do TPL do arquivo FNT e dos TPL que estão dentro do EFF.
<br>Agora você pode fazer repack usando o arquivo IdxuhdBin/IdxuhdBinBig, no qual vai criar um arquivo Bin valido sem modelo 3d.
<br>Feito melhorias no código.
<br>Renomeado os nomes das tools para colocar TPL no nome.

**Update: B.1.3.0**
<br> Corrigido bug: quando a quantidade de combinações de WeightMap é superior a 255, na qual estava sendo colocado o conteúdo de maneira errada no aquivo BIN.
<br> ATENÇÃO: arquivos BIN com a quantidade de combinações de WeightMap superior a 255 não funcionam no jogo base, eles crasham(fecham) o jogo.
<br> Para o arquivo BIN funcionar no jogo, deve estar com a DLL do Qingsheng (X3DAudio1_7.dll), com a opção "Allocate more memory for bones" ativada.
<br> Nota: quando o limite for superior ao permitido, o programa vai exibir um aviso no console.

**Update: B.1.2.3**
<br>Adicionado suporte para as versões big endians PS3 e X360;
<br>Nota: no lugar do arquivo "idxuhdbin" será gerado o arquivo "idxuhdbinbig", pois o conteúdo é incompatível entre esses dois arquivos.

**Update: B.1.2.0**
<br> Correção: arrumados os Ids dos bones com numeração maior que 128 que anteriormente ficavam com valor negativo;
<br> Correção: Arrumados os problemas com bones com Ids repetidos.
<br> Nota: bones com Ids maiores que 254 são inválidos;
<br> Melhoria: melhorado a velocidade do repack, agora é muito rápido fazer o repack.
<br> Correção: corrigido o "width X height" no TPL que estava invertido nas versões anteriores. A ordem correta no arquivo é "height X width";
<br> E foram feitas melhorias gerais no código;

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

## JADERLINK_RE4_\*\*_BIN_TPL_TOOL.exe

Programa responsável por extrair e recompilar os arquivos '.bin'/'.tpl';
<br> Segue abaixo os "inputs" e "outputs" do programa:
<br>Nota: o programa pode receber um ou dois arquivos como parâmetro;

* **JADERLINK_RE4_\*\*_BIN_TPL_TOOL.exe "file.BIN"**
    <br>Extrai o arquivo bin vai gerar os arquivos: 'file.obj', 'file.smd', 'file.idxmaterial' e 'file.idxuubin';
    <br>Caso na pasta tenha um arquivo '.tpl' de mesmo nome do bin, será considerado como se tivesse passado o arquivo também como parâmetro;
* **JADERLINK_RE4_\*\*_BIN_TPL_TOOL.exe "file.TPL"**
    <br>Extrai o arquivo tpl vai gerar o arquivo: 'file.idxuhdtpl';
* **JADERLINK_RE4_\*\*_BIN_TPL_TOOL.exe "file.BIN" "file.TPL"**
    <br>Ira gera os arquivos citados anteriormente mais o arquivo: 'file.mtl';
* **JADERLINK_RE4_\*\*_BIN_TPL_TOOL.exe "file.OBJ"**
    <br>Faz repack do arquivo '.bin', requisita na mesma pasta o arquivo '.idxuubin' de mesmo nome e o arquivo '.mtl' de mesmo nome;
    <br>Nota: você pode passar como segundo parâmetro o arquivo '.mtl' ou '.idxuhdtpl/.tpl', o resultado da operação é o mesmo citado acima;
    <br>Nota2: as operações com o '.mtl' envolvido requisitam que na mesma pasta do mtl tenha as texturas na qual ele faz referência;
    <br>Nota3: a partir da versão B.1.0.8, para usar o arquivo '.idxuhdtpl'/'.tpl', você deve passá-lo como segundo parâmetro.
* **JADERLINK_RE4_\*\*_BIN_TPL_TOOL.exe "file.OBJ" "file.idxmaterial"**
    <br>Faz repack do arquivo .bin, usando os arquivo '.obj' e .idxmaterial, requisita somente na mesma pasta o arquivo '.idxuubin';
* **JADERLINK_RE4_\*\*_BIN_TPL_TOOL.exe "file.SMD"**
    <br> Mesma explicação que do arquivo '.obj', so que agora fazendo o repack usando o arquivo '.smd';
* **JADERLINK_RE4_\*\*_BIN_TPL_TOOL.exe "file.SMD" "file.idxmaterial"**
    <br>O mesmo que expliquei acima.
* **JADERLINK_RE4_\*\*_BIN_TPL_TOOL.exe "file.MTL"**
    <br>"Extrai" o arquivo '.mtl' cria os arquivos: 'File.Repack.idxmaterial' e 'File.Repack.idxuhdtpl'
    <br>Nota: você pode passar como segundo parâmetro o arquivo '.tpl'/'.idxuhdtpl', no qual ele vai usar como referência para ordenar as texturas no tpl;
    <br>Nota2: as operações com o '.mtl' envolvido requisitam que na mesma pasta do mtl tenha as texturas na qual ele faz referência;
* **JADERLINK_RE4_\*\*_BIN_TPL_TOOL.exe "file.tpl" "file.idxmaterial"**
    <br> Cria o arquivo '.mtl';
* **JADERLINK_RE4_\*\*_BIN_TPL_TOOL.exe "file.idxuhdtpl" "file.idxmaterial"**
    <br> Também cria o arquivo '.mtl';
* **JADERLINK_RE4_\*\*_BIN_TPL_TOOL.exe "file.IDXUUBIN"**
    <br>Faz repack do arquivo BIN, sem modelo 3D (o modelo 3D fica invisível no jogo) 

## Explicação para que serve cada arquivo:

* .BIN: esse é o modelo 3d do jogo.
* .TPL: esse é o arquivo que guarda a referência das texturas usadas no modelo.
* .OBJ: modelo 3d que pode ser editado em um editor 3d;
* .MTL: arquivo que contém os materiais para serem carregados no editor 3d;
* .SMD: (StudioModel Data) modelo 3d que pode ser editado em um editor 3d (com suporte para bones);
* .IDXUUBIN: arquivo necessário para recompilar o arquivo .BIN
* .IDXUHDTPL: é a versão editável do arquivo .TPL;
* .IDXMATERIAL: é o arquivo que contém os materiais presentes no .bin (pode ser editado);
* .VTA: Quando tem morph, é um arquivo adicional que acompanha o SMD que tem o morph dentro (Atualmente não serve para o repack)
* \_morph\_xx.OBJ: arquivos obj que contem o morph. (Atualmente não serve para o repack)

## Arquivo .IDXUUBIN
Arquivo com configurações adicionais para o repack
<br>veja sobre aqui: [RE4 UHD BIN TPL TOOLS - Documentação: IDXUUBIN](https://jaderlink.blogspot.com/2025/08/RE4-TOOLS-DOCUMENTACAO-IDXUUBIN.html)

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

Para arrumar a ordem dos ids dos bones nos arquivos smd, depois de serem exportados do blender ou outro software de edição de modelos,<del> usar o programa: GC_GC_Skeleton_Changer.exe (procure o programa no fórum do re4, remod)</del>
<br>Veja: [SMD_BONE_TOOLS](https://github.com/JADERLINK/SMD_BONE_TOOLS)

## Carregando as texturas no arquivo .SMD

No blender para carregar o modelo .SMD com as texturas, em um novo "projeto", importe primeiro o arquivo .obj para ele carregar as texturas, delete o modelo do .obj importado, agora importe o modelo .smd, agora ele será carregado com as texturas.
<br>Lembrando também que as texturas devem estar na pasta com o nome de seu arquivo .PACK e essa pasta deve estar ao lado do arquivo .mtl;

## Código de terceiro:

[ObjLoader by chrisjansson](https://github.com/chrisjansson/ObjLoader):
Encontra-se em "RE4_UHD_BIN_TOOL\\CjClutter.ObjLoader.Loader", código modificado, as modificações podem ser vistas aqui: [link](https://github.com/JADERLINK/ObjLoader).

**At.te: JADERLINK**
<br>Thanks to \"mariokart64n\" and \"CodeMan02Fr\"
<br>Material information by \"Albert\"
<br>2025-08-24