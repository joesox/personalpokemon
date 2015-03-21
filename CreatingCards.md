#How to Create a Card using XML.

# Introduction #

Personal Pokemon (PPokemon) reads XML files and converts that data into Pokemon Card objects.  This page explains how you can create your own Cards and Decks using XML.


## CardType (case insensitive) ##
```
Null
Basic
Stage1
Stage2
LevelUp
Energy
Trainer
Supporter
Stadium
(default is Null)
```

## CardEventName (case insensitive) ##
`CardEventName` is an attribute found in the `CardDirections` Element.
XML path:`/Card/Attacks/Attack/CardDirections`
There needs to be one `CardDirections` line for each action in the directions.  These lines must be in the correct sequence listed from top to bottom in the xml. For example, the value "HEADS = NEXT" for `FlipACoin` means 'Flip a coin if HEADS go to NEXT CardDirections' then the next `CardDirections` will hold the variables for the next action. Adding a '= NEXT' at the end of a value will tell the gameengine that the action is not completed and needs to evaluate the next `CardDirections` xml line.

|CardEventName|Value|Example|
|:------------|:----|:------|
|AddDamageToAttack|>number to add to opponent's damage counter<|`<CardDirections CardEventName="AddDamageToAttack">40</CardDirections>`|
|AddDamageToBenched|>qty of benched to select = number to add to opponent's damage counter<|`<CardDirections CardEventName="AddDamageToBenched">2 = 10</CardDirections>`|
|AddDamageToEachBenched|>number to add to opponent's damage counter<|`<CardDirections CardEventName="AddDamageToEachBenched">10</CardDirections>`|
|AddDamageToSelf|>number to add to attacking pokemon's damage counter<|`<CardDirections CardEventName="AddDamageToSelf">30</CardDirections>`|
|AddSpecialCToAttack|>special condition<|`<CardDirections CardEventName="AddSpecialCToAttack">Paralyzed</CardDirections>`|
|AttachCardToAPokemon|>CardType<|`<CardDirections CardEventName="AttachCardToAPokemon">Energy</CardDirections>`|
|ChooseCardFromHandPlaceInDeck|  | |
|ChooseOpponPokemon|>Location<|`<CardDirections CardEventName="ChooseOpponPokemon">Benched</CardDirections>`|
|ChoosePokemon|  | |
|DiscardAnEnergy|>Location<(discard any Energy type) OR >Location = PokemonTYPE<(discard a specific Energy card) OR ><(discards first Energy card from attacking pokemon) |`<CardDirections CardEventName="DiscardAnEnergy">attacking = Grass</CardDirections>`|
|DiscardSpecialEnergyCard|  | |
|DrawACard|>times to draw a card from top of the deck (can leave blank if 1 time)<|`<CardDirections CardEventName="DrawACard">5</CardDirections>`|
|FlipACoin|>result = command<|`<CardDirections CardEventName="FlipACoin">HEADS = NEXT</CardDirections>`|
|IfActivePokemon|  | |
|IfDamaged|>command<|`<CardDirections CardEventName="IfDamaged">NEXT</CardDirections>`|
|IfDefendingDamaged|>command<|`<CardDirections CardEventName="IfDefendingDamaged">NEXT</CardDirections>`|
|IfDefendingHasPowers|  | |
|IfFlipHeads|  | |
|IfFlipTails|  | |
|IfKnockedOut|  | |
|IfMorePrizeCards|`<CardDirections CardEventName="IfMorePrizeCards"></CardDirections>`| |
|IfLessEnergyThanDefending|`<CardDirections CardEventName="IfLessEnergyThanDefending"></CardDirections>`| |
|IfNextDamageLessThan|  | |
|IfNoSpecialC|`<CardDirections CardEventName="IfNoSpecialC"></CardDirections>`| |
|MinusAmountMultipliedByAttachedDamage|>number of damage<|`<CardDirections CardEventName="MinusAmountMultipliedByAttachedDamage">10</CardDirections>`|
|MultiplyDamageByNumberOfPokemonInPlay|>number of damage to multiply by this same Pokemon in play count<|`<CardDirections CardEventName="MultiplyDamageByNumberOfPokemonInPlay">30</CardDirections>`|
|MultiplyByDamageOnSelf|>number of damage points to multiply count by<|`<CardDirections CardEventName="MultiplyByDamageOnSelf">10</CardDirections>`|
|MultiplyByNonUsedEnergyCount|>PokemonTYPE<|`<CardDirections CardEventName="MultiplyByNonUsedEnergyCount">Water</CardDirections>`|
|None|  | |
|OpponentChooseCardsFromDrawn|>number of cards for opponent to choose<|`<CardDirections CardEventName="OpponentChooseCardsFromDrawn">3</CardDirections>`|
|OpponentNextAttack|  | |
|RemoveDamageCounter|>int< OR >DAMAGEDONE<|`<CardDirections CardEventName="RemoveDamageCounter">2</CardDirections>`|
|SearchDeckForBasic|>PokemonTYPE<|`<CardDirections CardEventName="SearchDeckForBasic">Grass</CardDirections>`|
|SearchDeckForBasicOrEvolution|>int<|`<CardDirections CardEventName="SearchDeckForBasicOrEvolution">1</CardDirections>`|
|SearchDeckForBasicPlaceToBench|>PokemonTYPE<|`<CardDirections CardEventName="SearchDeckForBasicPlaceToBench">None</CardDirections>`|
|SearchDeckForEnergy|>PokemonTYPE< OR >PokemonTYPE = Location<|`<CardDirections CardEventName="SearchDeckForEnergy">Grass = hand</CardDirections>`|
|SearchDeckForPokemon|>true if stop at first one<|`<CardDirections CardEventName="SearchDeckForPokemon">true</CardDirections>`|
|SelectAllPokemonType|>Location = PokemonTYPE<|`<CardDirections CardEventName="SelectAllPokemonType">self = Grass</CardDirections>`|
|ShuffleDeck|`<CardDirections CardEventName="ShuffleDeck"></CardDirections>`| |
|SwitchDefendingWithBenched|`<CardDirections CardEventName="SwitchDefendingWithBenched"></CardDirections>`| |
|SwitchWithBenched|`<CardDirections CardEventName="SwitchWithBenched"></CardDirections>`| |
|(default is None)|  | |


## PokemonTYPE (case insensitive) ##
```
Colorless
Darkness
Fighting
Fire
Grass
Lightning
Metal
None
Null
Psychic
Water
(default is Null)
```

## Sample Card XML ##
```
  <Card description="Beedrill Stage2 (1/112)">
    <CardProperties HP="90" CardType="Stage2">
      <Pokemon PokemonType="Grass">Beedrill</Pokemon>
      <Resistance PokemonType="Null">0</Resistance>
      <RetreatCost PokemonType="Null">0</RetreatCost>
      <Weakness PokemonType="Fire">1</Weakness>
    </CardProperties>
    <Attacks>
      <Attack title="Poison Sting" body="The Defending Pokemon is now Poisoned.">
        <CardDirections CardEventName="AddSpecialCToAttack">Poisoned</CardDirections>
        <AttackProperties damage="20">
          <PokemonType qty="1">Grass</PokemonType>
        </AttackProperties>
      </Attack>
      <Attack title="Link Needle" body="This attack does 50 damage plus 30 more damage for each Beedrill (excluding this one) you have in play.">
        <CardDirections CardEventName="MultiplyDamageByNumberOfPokemonInPlay">+30</CardDirections>
        <AttackProperties damage="50">
          <PokemonType qty="1">Grass</PokemonType>
          <PokemonType qty="2">Colorless</PokemonType>
        </AttackProperties>
      </Attack>
    </Attacks>
  </Card>
```
## Sample 2 Card XML (with Poke-Power) ##
```
  <Card description="Exploud ex">
    <CardProperties HP="150" CardType="Stage2">
      <Pokemon PokemonType="Colorless">Exploud</Pokemon>
      <Resistance PokemonType="Null">0</Resistance>
      <RetreatCost PokemonType="Colorless">3</RetreatCost>
      <Weakness PokemonType="Fighting">0</Weakness>
    </CardProperties>
    <Power title="Extra Noise" body="As long as Exploud ex is your Active Pokemon, put 1 damage counter on each of your opponent's Pokemon-ex between turns.">
      <CardDirections CardEventName="IfActivePokemon"></CardDirections>
      <CardDirections CardEventName="ChooseOpponPokemon">ALL::ex</CardDirections>
    </Power>
    <Attacks>
      <Attack title="Derail" body="Discard a Special Energy card, if any, attached to the Defending Pokemon.">
        <CardDirections CardEventName="DiscardSpecialEnergyCard"></CardDirections>
        <AttackProperties damage="40">
          <PokemonType qty="2">Colorless</PokemonType>
        </AttackProperties>
      </Attack>
      <Attack title="Hyper Tail" body="If the Defending Pokemon has any Poke-Powers or Poke-Bodies, this attack does 60 damage plus 20 more damage.">
        <CardDirections CardEventName="IfDefendingHasPowers">NEXT</CardDirections>
        <CardDirections CardEventName="AddDamageToAttack">20</CardDirections>
        <AttackProperties damage="60">
          <PokemonType qty="3">Colorless</PokemonType>
        </AttackProperties>
      </Attack>
    </Attacks>
  </Card>
```