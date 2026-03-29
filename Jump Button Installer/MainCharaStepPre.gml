if (global.darkzone == 1)
    darkadjustforjumpmod = 1;
else
    darkadjustforjumpmod = 0.5;

if (jumpmodtimer >= -5)
{
    y -= (jumpmodtimer * 3 * darkadjustforjumpmod);
    jumpmodtimer -= 0.5;
    shadowofjumpmod.x = x;
    shadowofjumpmod.visible = 1;
    
    if (keyboard_check_pressed(ord("D")) && jumpmoddoub < 1.1)
    {
        jumpmoddoub += 0.1;
        audio_sound_pitch(snd_jump, jumpmoddoub);
        snd_play(snd_jump);
        jumpmodtimer += 5;
        mask_index = spriteemptyjumpmod;
    }
    
    shadowofjumpmod.y += py;
    jumpmoddem = y;
    var i = jumpmodtimer;
    
    while (i >= -5)
    {
        jumpmoddem -= (i * 3 * darkadjustforjumpmod);
        i -= 0.5;
    }
    
    jumpmodshadowgo = jumpmoddem + (90 * darkadjustforjumpmod);
    
    if (shadowofjumpmod.y > jumpmodshadowgo)
    {
        shadowofjumpmod.y -= 10;
        
        if (shadowofjumpmod.y < jumpmodshadowgo)
            shadowofjumpmod.y = jumpmodshadowgo;
    }
    else if (shadowofjumpmod.y < jumpmodshadowgo)
    {
        shadowofjumpmod.y += 10;
        
        if (shadowofjumpmod.y > jumpmodshadowgo)
            shadowofjumpmod.y = jumpmodshadowgo;
    }
    
    shadowofjumpmod.image_xscale = darkadjustforjumpmod;
    shadowofjumpmod.image_yscale = darkadjustforjumpmod;
    shadowofjumpmod.image_alpha = 0.9;
    shadowofjumpmod.depth = depth;
}
else
{
    mask_index = jumpmodcolmask;
    shadowofjumpmod.visible = 0;
    
    if (keyboard_check_pressed(ord("D")))
    {
        shadowofjumpmod.y = y + (90 * darkadjustforjumpmod);
        snd_play(snd_jump);
        jumpmodtimer = 5;
        jumpmoddoub = 1;
        mask_index = spriteemptyjumpmod;
    }
    
    if (place_meeting(x, y, obj_solidblock_ch1) || place_meeting(x, y, obj_soliddark_ch1))
    {
        if (darkadjustforjumpmod != 0.5)
        {
            if (global.hp[1] > 0 && array_length(global.hp) > 0 && visible == 1)
            {
                if (!audio_is_playing(snd_hurt1))
                {
                    audio_sound_pitch(snd_hurt1, random_range(0.9, 1.1));
                    snd_play(snd_hurt1);
                }
                
                global.facing = irandom_range(0, 3);
                global.inv = -1;
                target = 0;
                damage = irandom_range(1, 5);
                global.hp[1] -= damage;
                jumpmodhurtinwall = instance_create(x, y, obj_dmgwriter);
                jumpmodhurtinwall.damage = damage;
            }
            else
            {
                scr_gameover_ch1();
            }
        }
    }
    else
    {
        audio_sound_pitch(snd_hurt1, 1);
    }
}