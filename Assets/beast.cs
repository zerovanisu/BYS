using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beast : MonoBehaviour
{
    //インスペクター
    public float speed; //速度
    public float gravity; //重力
    public float jumpSpeed;//ジャンプする速度
    public float jumpHeight;//高さ制限
    public float jumpLimitTime;//ジャンプ制限時間
    public GroundCheck ground; //接地判定
    public GroundCheck head;//頭ぶつけた判定

    //プライベート変数
    private Animator anim = null;
    private Rigidbody2D rb = null;
    private bool isGround = false;
    private bool isJump = false;
    private bool isHead = false;
    private float jumpPos = 0.0f;
    private float jumpTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //設置判定を取得
        isGround = ground.IsGround();
        isHead = head.IsGround();

        //キー入力されたら行動する
        float horizontalKey = Input.GetAxis("Horizontal");
        float xSpeed = 0.0f;
        float ySpeed = -gravity;
        float verticalKey = Input.GetAxis("Vertical");
        if (isGround)
        {
            if (verticalKey > 0 && jumpTime < jumpLimitTime)
            {
                ySpeed = jumpSpeed;
                jumpPos = transform.position.y; //ジャンプした位置を記録する
                isJump = true;
                jumpTime = 0.0f;
            }
            else
            {
                isJump = false;
            }
        }
        else if(isJump)
        {
            //ジャンプボタンを押しているか
            bool pushUpKey = verticalKey > 0;
            //現在の高さが飛べる高さより下か
            bool canHeight = jumpPos + jumpHeight > transform.position.y;
            //ジャンプ時間が長くなりすぎていないか
            bool canTime = jumpLimitTime > jumpTime;

            //上記3つに加え頭が天井にぶつかっていないか
            if(pushUpKey && canHeight && canTime && !isHead)
            {
                ySpeed = jumpSpeed;
                jumpTime += Time.deltaTime;//ジャンプで上昇してる時間 += ジャンプ中に進んだゲーム内時間

            }
            else
            {
                isJump = false;
                jumpTime = 0.0f;
            }
            //rb.velocity = new Vector2(xSpeed, ySpeed);
        }

        if (horizontalKey > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            anim.SetBool("run", true);
            xSpeed = speed;
        }
        else if (horizontalKey < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            anim.SetBool("run", true);
            xSpeed = -speed;
        }
        else
        {
            anim.SetBool("run", false);
            xSpeed = 0.0f;
        }
        anim.SetBool("jump", isJump);
        anim.SetBool("ground", isGround);
        rb.velocity = new Vector2(xSpeed, ySpeed);
    }
}
