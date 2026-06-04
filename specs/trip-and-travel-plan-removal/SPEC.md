# SPEC.md

## Feature

Remoção de Viagens e Planos de Viagem

---

## Objetivo

Permitir que o usuário remova viagens (Trips) e planos de viagem (TravelPlans) existentes, com confirmação explícita antes da exclusão e redirecionamento adequado após a operação.

---

## Requisitos de negócio

- O usuário deve poder excluir uma viagem a partir da tela de detalhes da viagem
- O usuário deve poder excluir um plano de viagem individual a partir da tela de visualização do plano
- Antes de excluir, deve ser exibida uma confirmação para o usuário
- Ao excluir uma viagem, todos os planos de viagem associados devem ser excluídos automaticamente
- Ao excluir um plano de viagem, apenas o plano deve ser removido — a viagem permanece intacta
- Após excluir uma viagem, o usuário deve ser redirecionado para o dashboard (lista de viagens)
- Após excluir um plano de viagem, o usuário deve ser redirecionado para a tela de detalhes da viagem
- A exclusão de um plano de viagem em status "Processing" não deve ser permitida

---

## Restrições

- A exclusão é definitiva (hard delete) — não há lixeira nem recuperação
- A exclusão de viagem utiliza cascade delete já configurado no banco para remover planos associados
- A exclusão de plano de viagem deve ser bloqueada enquanto o status for "Processing"
- A confirmação deve ser obtida antes de executar a exclusão

---

## Critérios de Aceitação

- O botão "Excluir" aparece na tela de detalhes da viagem ao lado do botão "Editar"
- O botão "Excluir" aparece na tela de visualização do plano de viagem
- Um diálogo de confirmação é exibido antes da exclusão
- Confirmar a exclusão remove o registro e redireciona o usuário
- Cancelar a exclusão mantém o usuário na tela atual
- Excluir uma viagem que possui planos remove a viagem e todos os seus planos
- Excluir um plano de viagem em status "Processing" exibe uma mensagem e bloqueia a operação
- Excluir uma viagem inexistente não causa erro — apenas retorna ao dashboard
- Excluir um plano de viagem inexistente não causa erro — apenas retorna aos detalhes da viagem
