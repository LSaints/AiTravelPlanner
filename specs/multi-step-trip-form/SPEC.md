# SPEC.md

## Feature

Formulário Multi-etapas de Nova Viagem

---

## Objetivo

Transformar o formulário único de criação de viagem em um formulário multi-etapas com navegação progressiva, dividido em categorias lógicas (Destino, Orçamento, Detalhes), melhorando a experiência do usuário em dispositivos móveis ao reduzir a carga cognitiva por etapa.

---

## Requisitos de negócio

- O formulário deve ser dividido em etapas sequenciais e independentes, cada uma agrupando campos relacionados
- O usuário deve navegar entre as etapas através de botões "Próximo" e "Voltar"
- O usuário deve visualizar claramente em qual etapa está e quantas etapas faltam (indicador de progresso)
- Os dados preenchidos em etapas anteriores devem ser preservados ao navegar entre etapas
- A validação dos campos deve ocorrer por etapa: o usuário só avança se a etapa atual estiver válida
- O envio final (submit) deve ocorrer apenas na última etapa, consolidando todos os dados
- O formulário deve funcionar perfeitamente em dispositivos móveis (mobile-first), com campos e botões ocupando 100% da largura disponível
- O design deve seguir o tema visual existente (Dark Cartoon Theme) com consistência de cores, bordas, tipografia e sombras
- O indicador de progresso deve mostrar claramente as etapas concluídas, a etapa atual e as etapas futuras
- O usuário pode cancelar a operação a qualquer momento e retornar à página inicial
- O comportamento de salvamento (loading state, redirecionamento) deve ser mantido idêntico ao fluxo atual

---

## Restrições

- Não deve alterar o modelo de dados existente (`Trip`, `CreateTripRequest`, `CreateTrip`)
- Não deve alterar a rota da página (`/trips/new`)
- Deve reutilizar o `TripFormModel` existente como modelo compartilhado entre as etapas
- O submit final deve utilizar o mesmo `CreateTrip UseCase` existente
- Deve respeitar o layout mobile-first já estabelecido nos breakpoints da aplicação
- As animações devem seguir o padrão existente (fadeInDown, fadeInUp)

---

## Critérios de Aceitação

- O formulário é exibido em 3 etapas distintas: Destino, Orçamento, Detalhes
- A primeira etapa (Destino) contém os campos: Destino, País, Data de Início, Data de Término
- A segunda etapa (Orçamento) contém os campos: Pessoas, Orçamento
- A terceira etapa (Detalhes) contém os campos: O que pretende fazer, Observações adicionais
- O indicador de progresso mostra 3 etapas com identificação visual clara da etapa atual
- O botão "Próximo" na etapa 1 leva à etapa 2, e da etapa 2 leva à etapa 3
- O botão "Voltar" na etapa 2 leva à etapa 1, e na etapa 3 leva à etapa 2
- Na etapa 3, o botão de submit exibe "Salvar Viagem" e aciona o salvamento
- A validação impede avançar se campos obrigatórios da etapa atual não forem preenchidos
- Mensagens de validação são exibidas por campo, conforme padrão existente
- O estado de carregamento ("Salvando...") é exibido durante o submit na etapa 3
- Em viewport mobile (< 640px), todos os campos e botões ocupam 100% da largura
- O formulário mantém a aparência visual consistente com o tema Dark Cartoon existente
- O cancelamento redireciona para a página inicial (`/`)
