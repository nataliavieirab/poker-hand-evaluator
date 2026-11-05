# Validador de Mão de Poker

## Especificação

Desenvolver um componente capaz de avaliar as mãos de dois jogadores de Poker, classificar cada uma delas e informar qual jogador é o vencedor. Uma mão é formada por exatamente cinco cartas. O componente deve ser implementado na camada de domínio da aplicação.

## 1. Requisitos Funcionais

O componente deve receber as mãos de dois jogadores como entrada e retornar o vencedor, indicando a classificação de cada mão.

Cada carta é representada por dois caracteres: o valor seguido do naipe. As cartas de uma mão são separadas por espaço, por exemplo: `5C 5P 6E 7P KO`.

### 1.1. Valores das cartas

| Valor        | Símbolo                | Ordem               |
| ------------ | ---------------------- | ------------------- |
| Dois a Nove  | `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9` | `2` a `9` |
| Dez          | `D`                    | 10                  |
| Valete       | `J`                    | 11                  |
| Dama         | `Q`                    | 12                  |
| Rei          | `K`                    | 13                  |
| Ás           | `A`                    | 14 (carta mais alta) |

### 1.2. Naipes

| Naipe    | Símbolo |
| -------- | ------- |
| Ouro     | `O`     |
| Copas    | `C`     |
| Espadas  | `E`     |
| Paus     | `P`     |

### 1.3. Classificações das mãos

O componente deve identificar a classificação de cada mão, da mais baixa para a mais alta:

| Ranking | Classificação    | Descrição                                                    |
| :-----: | ---------------- | ------------------------------------------------------------ |
| 1       | Carta Alta       | Nenhuma combinação; vale a carta de maior valor              |
| 2       | Um Par           | Duas cartas do mesmo valor                                   |
| 3       | Dois Pares       | Dois pares de valores diferentes                             |
| 4       | Trinca           | Três cartas do mesmo valor e duas de valores diferentes      |
| 5       | Sequência (*Straight*) | Cinco cartas com valores consecutivos                 |
| 6       | Flush            | Cinco cartas do mesmo naipe                                  |
| 7       | Full House       | Uma trinca e um par                                          |
| 8       | Quadra           | Quatro cartas do mesmo valor                                 |
| 9       | Straight Flush   | Cinco cartas consecutivas e do mesmo naipe                   |
| 10      | Royal Flush      | A sequência 10, J, Q, K, A do mesmo naipe                    |

### 1.4. Comparação das mãos

| Situação                   | Resultado Esperado                                                         |
| -------------------------- | -------------------------------------------------------------------------- |
| Classificações diferentes  | Vence a mão de classificação mais alta                                     |
| Mesma classificação        | Vence a mão formada pelas cartas de maior valor (ver Regras de Desempate)  |
| Mãos idênticas em valor    | Empate                                                                     |

## 2. Regras de Comparação e Validação

### 2.1. Classificação mais alta vence

Quando as mãos possuem classificações diferentes, o vencedor é sempre o jogador com a classificação de ranking mais alto, independentemente do valor das cartas.

**Exemplos:**

- Flush (6) vence Trinca (4), mesmo que a trinca seja de Ases.
- Um Par de Dois (2) vence Carta Alta com Ás (1).

### 2.2. Desempate entre mãos de mesma classificação

Quando as mãos possuem a mesma classificação, compara-se primeiro as cartas que formam a combinação e, em seguida, as cartas restantes (*kickers*), da maior para a menor.

| Classificação    | Critério de Desempate                                                                |
| ---------------- | ------------------------------------------------------------------------------------ |
| Carta Alta       | Carta mais alta; persistindo o empate, a próxima carta mais alta, e assim por diante |
| Um Par           | Valor do par; depois as três cartas restantes, da maior para a menor                 |
| Dois Pares       | Valor do par mais alto; depois o par mais baixo; depois a carta restante             |
| Trinca           | Valor da trinca; depois as duas cartas restantes, da maior para a menor              |
| Sequência        | Carta mais alta da sequência                                                         |
| Flush            | Carta mais alta; persistindo o empate, as seguintes, da maior para a menor           |
| Full House       | Valor da trinca; depois o valor do par                                               |
| Quadra           | Valor da quadra; depois a carta restante                                             |
| Straight Flush   | Carta mais alta da sequência                                                         |
| Royal Flush      | Sempre empate                                                                        |

**Exemplos:**

- Par de Oito vence Par de Cinco.
- Par de Damas com 9 vence Par de Damas com 7.
- Full House com três 4 vence Full House com três 3.

### 2.3. Naipes não desempatam

Os naipes não têm valor entre si. Se todas as cartas de ambas as mãos possuem os mesmos valores, o resultado é **Empate**.

**Exemplo:**

- `2O 5C 8E JP AO` × `2P 5E 8C JO AC` → **Empate**

### 2.4. O Ás é sempre a carta mais alta

O Ás vale 14 e nunca vale 1. A sequência `A 2 3 4 5` não é considerada uma sequência válida (opcional: aceitar como sequência mais baixa).

### 2.5. Mãos inválidas devem ser rejeitadas

O componente deverá lançar uma exceção ou retornar uma mensagem de erro apropriada quando a mão:

- possuir quantidade de cartas diferente de cinco;
- contiver uma carta repetida (mesmo valor e mesmo naipe);
- contiver um valor ou naipe inexistente (ex.: `1C`, `7S`, `10O`);
- compartilhar uma carta com a mão do outro jogador.

**Exemplos:**

- `5C 5P 6E 7P` → Erro (quatro cartas)
- `5C 5C 6E 7P KO` → Erro (carta repetida)
- `5C 5P 6E 7S KO` → Erro (naipe `S` não existe)

## 3. Exemplos de Entrada e Saída

| Jogador 1      | Jogador 2      | Saída Esperada                                                          |
| -------------- | -------------- | ----------------------------------------------------------------------- |
| `5C 5P 6E 7P KO` | `2P 3E 8E 8O DO` | Jogador 2 (Par de Oito vence Par de Cinco)                            |
| `5O 8P 9E JE AP` | `2P 5P 7O 8E QC` | Jogador 1 (Carta Alta Ás vence Carta Alta Dama)                       |
| `2O 9P AE AC AP` | `3O 6O 7O DO QO` | Jogador 2 (Flush vence Trinca)                                        |
| `4O 6E 9C QC QP` | `3O 6O 7C QO QE` | Jogador 1 (Par de Damas; kicker 9 vence kicker 7)                     |
| `2C 2O 4P 4O 4E` | `3P 3O 3E 9E 9O` | Jogador 1 (Full House com três 4 vence três 3)                        |
| `3O 4C 5E 6P 7O` | `2C 3E 7P 7E 7C` | Jogador 1 (Sequência vence Trinca)                                    |
| `9O 9C 9E 9P 2O` | `2C 2E 7P 7O 7C` | Jogador 1 (Quadra vence Full House)                                   |
| `4E 5E 6E 7E 8E` | `9O 9C 9E 9P 2O` | Jogador 1 (Straight Flush vence Quadra)                               |
| `DC JC QC KC AC` | `5E 6E 7E 8E 9E` | Jogador 1 (Royal Flush vence Straight Flush)                          |
| `4O 4C 8E 8P KO` | `4E 4P 8O 8C QO` | Jogador 1 (Dois Pares iguais; kicker K vence Q)                       |
| `2O 5C 8E JP AO` | `2P 5E 8C JO AC` | Empate                                                                |
| `5C 5P 6E 7P`    | `2P 3E 8E 8O DO` | Erro (mão com quatro cartas)                                          |
| `5C 5P 6E 7S KO` | `2P 3E 8E 8O DO` | Erro (naipe inválido)                                                 |
| `5C 5C 6E 7P KO` | `2P 3E 8E 8O DO` | Erro (carta repetida)                                                 |

## 4. Critérios de Aceitação

- [ ] O componente deve aceitar as mãos de dois jogadores, com cinco cartas cada, no formato especificado.
- [ ] O componente deve identificar corretamente as dez classificações de mão.
- [ ] O componente deve informar o vencedor (Jogador 1, Jogador 2 ou Empate) e a classificação de cada mão em português.
- [ ] O componente deve aplicar as regras de desempate quando as mãos tiverem a mesma classificação.
- [ ] O componente deve rejeitar mãos inválidas (quantidade de cartas, cartas repetidas, valores ou naipes inexistentes).
- [ ] O componente deve possuir testes unitários cobrindo todos os cenários apresentados nesta especificação.
