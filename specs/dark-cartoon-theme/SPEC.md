# SPEC.md

## Feature

Tema Escuro Cartoon

---

## Objetivo

Implementar um tema visual escuro com estilo cartoon para toda a aplicação, utilizando laranja como cor primária e criando uma identidade visual lúdica, moderna e consistente.

---

## Requisitos de negócio

- A aplicação deve utilizar um esquema de cores escuras como padrão (fundos escuros, textos claros)
- A cor laranja deve ser a cor primária da identidade visual, aplicada em elementos interativos, botões, links, destaques e componentes navegáveis
- O estilo visual deve ter aparência cartoon: cantos arredondados pronunciados, bordas chamativas (grossas e visíveis), sombras com deslocamento aparente e tipografia com peso bold
- Os componentes da interface (botões, cards, inputs, navegação, modais) devem seguir o estilo cartoon de forma consistente
- O tema deve ser aplicado globalmente sem necessidade de configuração por parte do usuário
- O tema deve ser responsivo e manter a legibilidade em todos os tamanhos de tela
- Ícones e ilustrações devem ser coerentes com o estilo cartoon (traços grossos, cores sólidas, formas simples)

---

## Restrições

- O tema deve ser implementado exclusivamente com Tailwind CSS v4 e CSS scoped (.razor.css), sem introdução de novas bibliotecas de estilização
- O CSS scoped existente dos componentes não deve ser removido — deve ser adaptado para usar variáveis do tema
- O tema não pode quebrar a funcionalidade existente da aplicação
- O tema deve conviver com o CSS scoped dos componentes sem conflitos
- A paleta de cores deve ser definida em um único ponto central (variáveis CSS ou configuração Tailwind)

---

## Critérios de Aceitação

- O fundo da aplicação é escuro (#1a1a2e ou equivalente) e o texto primário é claro (#f0f0f0 ou equivalente)
- Botões primários utilizam laranja (#ff6b35 ou equivalente) como cor de fundo com texto escuro para contraste
- Links e elementos interativos são destacados em laranja
- Cards, modais e painéis possuem cantos arredondados (border-radius >= 12px), bordas visíveis e sombras com deslocamento
- O menu de navegação lateral segue o estilo cartoon com fundo escuro e destaques em laranja
- A tipografia utiliza peso bold (700) nos títulos e elementos de navegação
- Inputs e formulários possuem bordas arredondadas e chamativas no estilo cartoon
- Um botão primário laranja com cantos arredondados e borda escura é renderizado corretamente
- A transição entre temas não causa flicker ou quebra de layout
